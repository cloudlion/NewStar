#!/usr/bin/python

import commands
import subprocess
import os
import sys

def process(parameter):
    pr = subprocess.Popen(parameter, shell = True, stdout = subprocess.PIPE, stderr = subprocess.PIPE)
    (out, error) = pr.communicate()

    print "Error : " + str(error)
    print "out : " + str(out)

CWD = os.path.normpath(os.path.abspath(os.path.dirname(os.path.realpath(__file__))))
ROOT_PATH = os.path.normpath(os.path.abspath(os.path.join(CWD, '../../../')))

command = os.path.join(CWD, 'astyle/build/mac/bin/astyle')
options = " --options=" + os.path.join(CWD, 'csharpformat.astylerc')
fe_dir = " \"" + ROOT_PATH + '/mad_max_fe/*.cs\"'
art_dir = " \"" + ROOT_PATH + '/mad_max_art/*.cs\"'

process(command + options + fe_dir)
process(command + options + art_dir)
