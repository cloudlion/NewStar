#!/usr/bin/env python
# -*- coding:utf-8 -*-

import os
import sys

def pause_exit(msg):
    print msg
    raw_input('press any key to exit')
    os.exit(0)

class Info:
    def __init__(self, name, id, server):
        self.name = name
        self.id = id
        self.server = server

def parse_package(lines, curline):
    line = lines[curline].strip().rstrip(';').rstrip()
    _, name = line.split()
    return curline + 1, name
    
def parse_message(lines, curline):
    line = lines[curline].strip().rstrip('{').rstrip()
    _, name = line.split()
    curline += 1
    block = 1
    id = ""
    server = ""
    while(True):
        if curline >= len(lines):
            pause_exit('parse message error:' + name)

        line = lines[curline].strip()
        if line.startswith('//'):
            if block == 1:
                line = line.lstrip('/').lstrip()
                if line.startswith('id'):
                    _, id = line.split('=')
                elif line.startswith('server'):
                    _, server = line.split('=')
        elif line.startswith('message') or line.startswith('enum'):
            block += 1
        elif line.startswith('}'):
            block -= 1
            if block == 0:
                break
        else:
            pass
        curline += 1
    
    info = Info(name.strip().upper(), id.strip(), server.strip())
    return curline, info
    
def parse_file(file):
    f = open(file)
    if not f:
        pause_exit('open file failed' + file)

    lines = f.readlines()
    f.close()

    curline = 0
    dic = {}
    pkname = ''
    vals = []
    while(True):
        if curline >= len(lines):
            break
        line = lines[curline].strip()
        if not line:
            curline += 1
            continue
        if line.startswith('package'):
            curline, pkname = parse_package(lines, curline)
            continue
        elif line.startswith('message'):
            curline, info = parse_message(lines, curline)
            vals.append(info)
            continue
        else:
            curline += 1
            continue
    pkname = os.path.splitext(os.path.split(file)[1])[0]
    dic[pkname] = vals
    return dic

def parse_dir(src_dir):
    dic = {}
    for src in os.listdir(src_dir):
        if os.path.isdir(src):
            dic.update(parse_dir(os.path.join(src_dir, src)))
        else:
            f, e = os.path.splitext(src)
            if e == '.proto':
                dic.update(parse_file(os.path.join(src_dir, src)))
    return dic

def output(dic, id_file, rt_file):
    f = open(id_file, 'wb')
    if not f:
        pause_exit('open file failed' + id_file)

    f.write('//code generated by parse\n\n')
    f.write('package proto\n\n')
    f.write('const (')
    for k in dic.keys():
        f.write('\n\t//' + k + '\n')
        for i in dic[k]:
            if i.id:
                f.write('\t' + i.name + ' = ' + i.id + '\n')
    f.write(')')
    f.flush()

    f.close()

    f = open(rt_file, 'wb')
    if not f:
        pause_exit('open file failed' + id_file)

    f.write('//code generated by parse\n\n')
    f.write('package proto\n\n')
    f.write('var RouteMap map[int] string\n\n')
    f.write('func init() {\n')
    f.write('\tRouteMap = map[int]string {\n')
    for k in dic.keys():
        f.write('\n\t\t//' + k + '\n')
        for i in dic[k]:
            if i.server:
                f.write('\t\t' + i.name + ' : ' + '"' + i.server + '"' + ',\n')
    f.write('\t}\n')
    f.write('}\n')
    f.flush()

    f.close()
        
def parse(src_dir, id_file, rt_file):
    sd = os.path.normpath(os.path.abspath(src_dir))
    dd = os.path.normpath(os.path.abspath(id_file))
    ddp = os.path.split(dd)[0]
    dr = os.path.normpath(os.path.abspath(rt_file))
    drp = os.path.split(dr)[0]
    if not os.path.exists(sd):
        pause_exit('can not find src dir:' + sd)
    if not os.path.exists(ddp):
        os.makedirs(ddp)
    if not os.path.exists(drp):
        os.makedirs(drp)

    output(parse_dir(sd), dd, dr)

if __name__ == '__main__':
    if len(sys.argv) < 4:
        pause_exit('cmdline usage: parse src_folder id_file route_file')
    parse(sys.argv[1], sys.argv[2], sys.argv[3])
