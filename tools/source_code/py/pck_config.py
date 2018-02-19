#!/usr/bin/env python
# -*- coding:utf-8 -*-

"""pack game config xls to cs
   author: zhangshiliang
"""

import os
import sys
import xlrd
import struct
import re
import path_util
from mako.template import Template

BYTE_ORDER = '<'
ARR_SPLIT = '|'
STRUCT_SPLIT = '*'
CSV_SPLIT = ','
STRUCT_PREFIX = 's-'
STRUCT_MID = '-'
g_CfgData_List = []

class ParseException(Exception):
    def __init__(self, id, name, native_except):
        self.id = id
        self.name = name
        self.native_except = native_except

    def __str__(self):
        return "id: " + str(self.id) + ", name: " + str(self.name)

class StructData:
    def __init__(self, name, vals):
        self.name = name
        self.vals = vals

class CfgData:
    def __init__(self, name_list, type_list, cl_list, arr_list, data_list, 
                 st_define_list, st_list):
        self.name_list = name_list
        self.type_list = type_list
        self.cl_list = cl_list
        self.arr_list = arr_list
        self.data_list = data_list
        self.st_define_list = st_define_list
        self.st_list = st_list

def parse_xls(file_name):   
    book = xlrd.open_workbook(file_name)
    if book.nsheets <= 0:
        return

    nl, tl, cl, al, sdl, sl = __init_prop(book)
    dl = __init_data(book)

    data = CfgData(nl, tl, cl, al, dl, sdl, sl)
    return data

def exp_code_file(file_name, template, cfg_data, class_name, bin_file):
    dir, name = os.path.split(file_name)
    if (dir) and (not os.path.exists(dir)):
        os.makedirs(dir)

    file = open(file_name, 'w')
    templ = Template(filename = template,
                     input_encoding='utf-8',
                     output_encoding='utf-8',
                     default_filters=['decode.utf8'],
                     encoding_errors='replace')

    # type_list2 = []
    # struct_define_list2 = []
    # for v in cfg_data.type_list:
    #     type_list2.append(v.replace("-", "_"))
    # for v in cfg_data.st_define_list:
    #     struct_define_list2.append(StructData(v.name.replace("-", "_"), v.vals))

    txt = templ.render(type_list = cfg_data.type_list,
                       name_list = cfg_data.name_list,
                       client_list = cfg_data.cl_list,
                       arr_list = cfg_data.arr_list,
                       struct_define_list = cfg_data.st_define_list,
                       struct_list = cfg_data.st_list,
                       class_name = class_name,
                       bin_file_name = bin_file)
    #txt = txt.replace('\n', '')
    #txt = txt.replace('\r', '')
    file.write(txt)

    file.flush()
    file.close()

def exp_bin_file(file_name, cfg_data, class_name):
    dir, name = os.path.split(file_name)
    if (dir) and (not os.path.exists(dir)):
        os.makedirs(dir)

    bf = open(file_name, 'wb')
    bf.write(struct.pack(BYTE_ORDER + 'i', 0))
    bf.write(struct.pack(BYTE_ORDER + 'h', len(class_name)))
    bf.write(struct.pack(BYTE_ORDER + str(len(class_name)) + 's', class_name))
    bf.write(struct.pack(BYTE_ORDER + 'h', __col_cnt(cfg_data)))
    bf.write(struct.pack(BYTE_ORDER + 'i', len(cfg_data.data_list)))

    for data in cfg_data.data_list:
        try:
            val = __parse_data(data, cfg_data)
        except ParseException, pe:
            print "parse err: " + str(pe) + ', ' + class_name
            raise pe.native_except

        for i in range(len(cfg_data.cl_list)):
            if(cfg_data.cl_list[i].count('c') <= 0):
                continue

            item = val[i]
            if cfg_data.arr_list[i]:
                bf.write(struct.pack(BYTE_ORDER + 'h', len(item)))
                for elem in item:
                    __write_item(bf, cfg_data.type_list[i], elem)
            else:
                __write_item(bf, cfg_data.type_list[i], item)

            bf.flush()

    length = bf.tell()
    bf.seek(0)
    bf.write(struct.pack(BYTE_ORDER + 'i', length))
    bf.flush()
    bf.close()
    
def exp_csv_file(file_name, cfg_data):
    dir, name = os.path.split(file_name)
    if (dir) and (not os.path.exists(dir)):
        os.makedirs(dir)

    file = open(file_name, 'wt')

    lines = []
    line = ""
    for name in cfg_data.name_list:
        line = line + name + CSV_SPLIT
    line = line + '\n'
    lines.append(line)

    line = ""
    for ty in cfg_data.type_list:
        line = line + ty + CSV_SPLIT
    line = line + '\n'
    lines.append(line)

    line = ''
    for cs in cfg_data.cl_list:
        line = line + cs + CSV_SPLIT
    line = line + '\n'
    lines.append(line)

    for data in cfg_data.data_list:
        line = ""
        for i in range(len(data)):
            item = str(data[i])
            if item.startswith('string') or item.startswith(STRUCT_PREFIX) or item.startswith('float'):
                line = line + str(item) + CSV_SPLIT
            else:
                line = line + item.split('.')[0] + CSV_SPLIT
        line = line + '\n'
        lines.append(line)

    file.writelines(lines)
    file.flush()
    file.close()    

def __init_prop(book):
    sheet = book.sheet_by_index(0)

    #row 0 is comment
    name_list = [str(t.value).strip() for t in sheet.row(1)]
    type_list = [str(t.value).strip().lower() for t in sheet.row(2)]
    client_list = [str(t.value).strip().lower() for t in sheet.row(3)]
    arr_list = [t.endswith('[]') for t in type_list]

    st_define_list = []
    st_list = []
    for ty in type_list:
        if ty.startswith(STRUCT_PREFIX):
            name = ty.strip('[]')
            elems = name.split(STRUCT_MID)
            vals = []
            vals.extend(elems[2:])
            exist = False
            for sd in st_define_list:
                if sd.name == name.replace(STRUCT_MID, '_'):
                    exist = True
                    break
            if not exist:
                st_define_list.append(StructData(name.replace(STRUCT_MID, '_'), vals))
            st_list.append(StructData(name.replace(STRUCT_MID, '_'), vals))
        else:
            st_list.append(StructData('', []))
            
    return name_list, type_list, client_list, arr_list, st_define_list, st_list

def __init_data(book):
    data_list = []
    for sheet in book.sheets():
        if sheet.name.startswith('~'):
            continue

        for rx in range(4, sheet.nrows):
            item_list = [data.value for data in sheet.row(rx)]
            for i in range(len(item_list)):
                if type(item_list[i]) is unicode:
                    item_list[i] = item_list[i].encode('utf-8')

            if str(item_list[0]).startswith('#'):
                continue

            #first column empty means no need
            if not str(item_list[0]).strip():
                print 'invalid data in line: ' + str(rx)
                continue
            
            data_list.append(item_list)
    return data_list

def __col_cnt(cfg_data):
    cnt = 0
    for item in cfg_data.cl_list:
        if item.count('c') > 0:
            cnt += 1
    return cnt
    
def __write_item(file, type, value):
    if type.startswith('bool'):
        file.write(struct.pack(BYTE_ORDER + 'b', value))
    elif type.startswith('byte'):
        file.write(struct.pack(BYTE_ORDER + 'B', value))
    elif type.startswith('short'):
        file.write(struct.pack(BYTE_ORDER + 'h', value))
    elif type.startswith('int'):
        file.write(struct.pack(BYTE_ORDER + 'i', value))
    elif type.startswith('long'):
        file.write(struct.pack(BYTE_ORDER + 'q', value))
    elif type.startswith('float'):
        file.write(struct.pack(BYTE_ORDER + 'f', value))
    elif type.startswith('double'):
        file.write(struct.pack(BYTE_ORDER + 'd', value))
    elif type.startswith('string'):
        file.write(struct.pack(BYTE_ORDER + 'h', len(value)))
        if len(value) > 0:
            file.write(struct.pack(BYTE_ORDER + str(len(value)) + 's', value))
    elif type.startswith(STRUCT_PREFIX):
        ln = int(type.split(STRUCT_MID)[1])
        if not value.strip():
            for i in range(ln):
                file.write(struct.pack(BYTE_ORDER + 'i', 0))
        else:
            elems = value.strip().split(STRUCT_SPLIT)
            if len(elems) != ln:
                raise Exception, 'struct data error ' + type + ':' + value
            for e in elems:
                file.write(struct.pack(BYTE_ORDER + 'i', int(e)))
    else:
        raise Exception, "unknown type to write: " + type + ':' + value

def __convert_item(type, value):
    str_val = str(value).lower().strip()
    if type.startswith('bool'):
        if str_val == 'false' or str_val == '0' or not str_val:
            return 0
        elif str_val == 'true' or str_val == '1':
            return 1
        else:
            raise Exception, "invalid bool data: " + str_val
    elif type.startswith('byte'):
        if not str_val:
            value = 0
        return int(value)
    elif type.startswith('short'):
        if not str_val:
            value = 0
        return int(value)
    elif type.startswith('int'):
        if not str_val:
            value = 0
        return int(value)
    elif type.startswith('long'):
        if not str_val:
            value = 0
        return long(value)
    elif type.startswith('float'):
        if not str_val:
            value = 0
        return float(value)
    elif type.startswith('double'):
        if not str_val:
            value = 0
        return float(value)
    elif type.startswith('string'):
        return str(value)
    elif type.startswith(STRUCT_PREFIX):
        return str(value)
    else:
        raise Exception, "unknown type to convert: " + type

def __parse_data(elem_list, cfg_data):
    value_list = []
    for i in range(len(cfg_data.cl_list)):
        try:
            if(cfg_data.cl_list[i].count('c') <= 0):
                value_list.append(elem_list[i])
                continue

            item = elem_list[i]
            if(cfg_data.arr_list[i]):
                arr = []
                if not str(item).strip():
                    pass # do nothing
                if type(item) is not str:
                    value = __convert_item(cfg_data.type_list[i], item)
                    arr.append(value)
                else:
                    for elem in item.split(ARR_SPLIT):
                        if not elem.strip():
                            continue

                        value = __convert_item(cfg_data.type_list[i], elem.strip())
                        arr.append(value)
                value_list.append(arr)
            else:
                value = __convert_item(cfg_data.type_list[i], item)
                value_list.append(value)
        except Exception, inst:
            raise ParseException(str(elem_list[0]), cfg_data.name_list[i], inst)

    return value_list
   
def exp_mgr_code_file(file_name, template, cls_name_list):
    dir, name = os.path.split(file_name)
    if (dir) and (not os.path.exists(dir)):
        os.makedirs(dir)

    file = open(file_name, 'w')
    templ = Template(filename = template)
    txt = templ.render(class_list = cls_name_list)
    #txt = txt.replace('\n', '')
    #txt = txt.replace('\r', '')
    file.write(txt)

    file.flush()
    file.close()

def ensure_empty_folder(folder):
    if os.path.exists(folder):
        path_util.clear_folder(folder)
    else:
        os.makedirs(folder)

def pack_config_dir(flag, src_dir, cs_dir, bin_dir, csv_dir, go_dir):
    if not os.path.exists(src_dir):
        return
    flags = flag.split('|')
    if not flags:
        flags = ['cs', 'go', 'csv']
    if 'csv' in flags:
        ensure_empty_folder(csv_dir)
    if 'cs' in flags:
        ensure_empty_folder(cs_dir)
    if 'go' in flags:
        ensure_empty_folder(go_dir)
    if 'cs' in flags or 'go' in flags:
        ensure_empty_folder(bin_dir)
    cls_name_list = []
    for file in os.listdir(src_dir):
        if os.path.isfile(os.path.join(src_dir, file)) \
                and os.path.splitext(file)[1] == '.xls':
            print "pack file: " + file
            file_name = os.path.splitext(file)[0]
            class_name = file_name
            cs_file = file_name + '.cs'
            go_file = file_name + '.go'
            bin_file = file_name + '.bytes'
            csv_file = file_name + '.csv'
            cfg_data = parse_xls(os.path.join(src_dir, file))

            if 'csv' in flags:
                exp_csv_file(os.path.join(csv_dir, csv_file), cfg_data)

            if 'cs' in flags or 'go' in flags:
                exp_bin_file(os.path.join(bin_dir, bin_file), cfg_data, class_name)
            if 'cs' in flags:
                exp_code_file(os.path.join(cs_dir, cs_file),
                              os.path.join(MAKO_PATH, GAME_CONF_MAKO_CS), 
                              cfg_data, class_name, bin_file)
            # if 'go' in flags:
            #     exp_code_file(os.path.join(go_dir, go_file),
            #                   os.path.join(MAKO_PATH, GAME_CONF_MAKO_GO),
            #                   cfg_data, class_name, bin_file)

            cls_name_list.append(class_name)

    if 'cs' in flags:
        exp_mgr_code_file(os.path.join(cs_dir, "ConfFact.cs"),
                          os.path.join(MAKO_PATH, GAME_CONF_FACT_MAKO_CS), 
                          cls_name_list)
    # if 'go' in flags:
    #     exp_mgr_code_file(os.path.join(go_dir, "ConfFact.go"),
    #                       os.path.join(MAKO_PATH, GAME_CONF_FACT_MAKO_GO),
    #                       cls_name_list)

from config import *
if __name__ == '__main__':
    print 'usage: ./pck_config.py flags xls_path csv_path cs_path bin_path'
    print "pack conf begin..."
    flags = GAME_CONF_PCK_FLAG
    xls_path = SRC_GAME_CONF_PATH
    csv_path = EXP_GAME_CONF_CSV_PATH
    cs_path =  EXP_GAME_CONF_CS_PATH
    bin_path = EXP_GAME_CONF_BIN_PATH
    if len(sys.argv) >= 2:
        flags = sys.argv[1]
    if len(sys.argv) >= 3:
        xls_path = sys.argv[2]
    if len(sys.argv) >= 4:
        csv_path = sys.argv[3]
    if len(sys.argv) >= 5:
        cs_path = sys.argv[4]
    if len(sys.argv) >= 6:
        bin_path = sys.argv[5]
    pack_config_dir(flags, xls_path, cs_path, bin_path, csv_path, '')
    print "pack conf successful"
