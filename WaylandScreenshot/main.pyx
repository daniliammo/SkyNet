# From: https://gitlab.gnome.org/-/snippets/19

import re
import dbus
from dbus.mainloop.glib import DBusGMainLoop
import gi
gi.require_version('Gst', '1.0')
from gi.repository import GLib, Gst

import socket

# from threading import Thread

import time


cdef int frameCount = 0

# Поля
DBusGMainLoop(set_as_default = True)
cdef object loop = GLib.MainLoop()

# TCP
cdef str TCP_IP = '127.0.0.1'
cdef int TCP_PORT = 5254
cdef int BUFFER_SIZE = 262144
cdef object thread;
cdef object s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# DBus
cdef object bus = dbus.SessionBus()
cdef str request_iface = 'org.freedesktop.portal.Request'
cdef str screen_cast_iface = 'org.freedesktop.portal.ScreenCast'

cdef int request_token_counter = 0
cdef int session_token_counter = 0
cdef str sender_name = re.sub(r'\.', r'_', bus.get_unique_name()[1:])

cdef int tries = 0


# Методы
cpdef void main():
    Gst.init()

    try:
      s.connect((TCP_IP, TCP_PORT))
      print(f'Подключился к {TCP_IP}:{TCP_PORT}!')
    except ConnectionRefusedError:
      print(f'ConnectionRefusedError. Не удалось подключится к {TCP_IP}:{TCP_PORT}. Возможно SkyNet не запущен или работает на другом порте. Пробую еще раз...')
      time.sleep(1)
      global tries
      tries += 1
      print(f'Попытка {tries}')

      if(tries >= 7):
          print(f'Не удалось подключится к {TCP_IP}:{TCP_PORT} за 7 попыток. Остановка.')
          terminate(1)
      main()

    try:
        loop.run()
    except KeyboardInterrupt:
        terminate(0)

cdef void terminate(int code):
    try:
      thread.stop()
    except:
      pass

    s.close()
    loop.quit()
    exit(code)

cdef object new_request_path():
    global request_token_counter
    request_token_counter += 1
    token = f'u{request_token_counter}'
    path = f'/org/freedesktop/portal/desktop/request/{sender_name}/{token}'
    return path, token

cdef object new_session_path():
    global session_token_counter
    session_token_counter += 1
    token = f'u{session_token_counter}'
    path = f'/org/freedesktop/portal/desktop/session/{sender_name}/{token}'
    return path, token

def screen_cast_call(method, callback, *args, options={}):
    (request_path, request_token) = new_request_path()
    bus.add_signal_receiver(callback,
                            'Response',
                            request_iface,
                            'org.freedesktop.portal.Desktop',
                            request_path)
    options['handle_token'] = request_token
    method(*(args + (options, )),
           dbus_interface=screen_cast_iface)

cdef void send_tcp_data(object sink):
    cdef object sample = sink.emit("pull-sample")
    cdef object buf = sample.get_buffer()

    try:
        # Извлечение данных из buf
        # s.send(str.encode("bebraIEND"))
        # s.send(str.encode("atuikIEND"))
        # s.send(str.encode("p diddyIEND"))
        s.send(buf.extract_dup(0, buf.get_size()))
    except BrokenPipeError:
        print("BrokenPipeError. Похоже SkyNet отключен. Остановка.")
        terminate(1)

cdef object on_new_sample(object sink):
    global frameCount
    frameCount += 1
    print(frameCount)

    # #global thread
    # thread = Thread(target=send_tcp_data, args=(sink,))
    # thread.start()
    send_tcp_data(sink)
    terminate(0)
    return Gst.FlowReturn.OK

cdef void play_pipewire_stream(node_id):
    empty_dict = dbus.Dictionary(signature="sv")
    fd_object = portal.OpenPipeWireRemote(session, empty_dict,
                                          dbus_interface=screen_cast_iface)

    fd = fd_object.take()
    # Создаем конвейер с использованием appsink
    # avenc_bmp
    cdef object pipeline = Gst.parse_launch(
        f'pipewiresrc fd={fd} path={node_id} ! videoconvert ! pngenc ! appsink name=sink'
        )

    # Получаем объект appsink
    appsink = pipeline.get_by_name("sink")
    appsink.set_property("emit-signals", True)
    appsink.connect("new-sample", on_new_sample)

    # Указываем состояние PLAYING
    pipeline.set_state(Gst.State.PLAYING)

cdef void on_start_response(response, results):
    if response != 0:
        print(f'Failed to start: {response}')
        terminate(1)
        return

    print("streams:")
    for (node_id, stream_properties) in results['streams']:
        print("stream {}".format(node_id))
        play_pipewire_stream(node_id)

cdef void on_select_sources_response(response, results):
    if response != 0:
        print(f'Failed to select sources: {response}')
        terminate(1)
        return

    print("sources selected")
    global session
    screen_cast_call(portal.Start, on_start_response,
                     session, '')

cdef void on_create_session_response(response, results):
    if response != 0:
        print(f'Failed to create session: {response}')
        terminate(1)
        return

    global session
    session = results['session_handle']
    print(f"session {session} created")

    screen_cast_call(portal.SelectSources, on_select_sources_response,
                     session,
                     options={ 'multiple': False,
                               'types': dbus.UInt32(1|2) })

portal = bus.get_object('org.freedesktop.portal.Desktop',
                             '/org/freedesktop/portal/desktop')

(session_path, session_token) = new_session_path()
screen_cast_call(portal.CreateSession, on_create_session_response,
                 options={ 'session_handle_token': session_token })
