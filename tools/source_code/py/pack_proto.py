#!/usr/bin/env python

import os
from config import *
from path_util import *

MonoCmd = "/Applications/Unity/MonoDevelop.app/Contents/Frameworks/Mono.framework/Versions/2.10.11/bin/mono"
CsharpProtoGenCmd = "./ProtoGen/protogen.exe"

#def gen_cs(proto_gen_path, proto_gen, in_dir, out_dir):
#    pwd = os.getcwd()
#    os.chdir(proto_gen_path)
#    files = os.listdir(in_dir)
#    if not os.path.exists(out_dir):
#        os.makedirs(out_dir)
#    for file in files:
#        if file.endswith('.proto'):
#            os.system('cp ' + os.path.join(in_dir, file) + ' ./')
#            os.system("mono " + proto_gen
#                      + " -i:" + file
#                      + " -o:" + os.path.join(out_dir, file.replace('.proto', '.cs')))
#            os.system('rm ' + file)
#    os.chdir(pwd)
# 
#def gen_go(in_dir, out_dir):
#    files = os.listdir(in_dir)
#    for file in files:
#        if file.endswith('.proto'):
#            os.system("protoc "
#                      + ' --proto_path=' + in_dir
#                      + " --go_out=" + out_dir
#                      + " " + os.path.join(in_dir, file))

def gen_cs():
    recurse_clear_folder(DST_PROTO_CS_PATH, ['.cs'])
    pwd = os.getcwd()
    os.chdir(PROTO_PATH)
    print PROTO_PATH
    print PROTO_GENERATOR
    print PROTO_CONF_FILE
    print PROTO_FLAG
    print pwd
 #   os.system('./' + PROTO_GENERATOR + ' -c ' + PROTO_CONF_FILE + ' -t ' + PROTO_FLAG)
    os.system('./' + PROTO_GENERATOR + ' -c ' + PROTO_CONF_FILE + ' -t ' + PROTO_FLAG)
    command = ASTYLE_PATH + ' --quiet --options=' + ASTYLE_FORMAT_PATH + ' "' + DST_PROTO_CS_PATH + '/*.cs"'
    os.system(command)
    os.chdir(pwd)
    
if __name__ == '__main__':
    gen_cs()
