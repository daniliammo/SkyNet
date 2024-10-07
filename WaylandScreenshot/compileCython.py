
# Compile
from distutils.core import setup

from Cython.Build import cythonize
from Cython.Compiler import Options

Options.docstrings = False
setup(ext_modules = cythonize('main.pyx'))


# print OK
from colorama import init as colorama_init
from colorama import Fore
from colorama import Style

colorama_init()

print(f"{Fore.GREEN}Cython OK!{Style.RESET_ALL}")
