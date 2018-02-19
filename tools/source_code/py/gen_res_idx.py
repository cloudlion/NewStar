#!/usr/bin/env python
# -*- coding:utf-8 -*-

"""generate res index.
    author: zhang shi liang
"""
import os
import os.path as path
import sys
import gzip
import hashlib

from config import *
from path_util import *

def gen_res_idx():
    lines = []
    for file in os.listdir(EXP_RES_IDX_PATH):
        if file .endswith(RES_IDX_EXT_NAME):
            fc = open(path.join(EXP_RES_IDX_PATH, file), 'rb')
            if not fc:
                raise Exception, "can not open file: " + file
            fl = fc.readlines()
            lines.extend(fl)
    fo = open(path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE), 'wb')
    if not fo:
        raise Exception, "can not open file: " + EXP_RES_IDX_FILE
    fo.writelines(lines)
    fo.close()

def gen_bundle_dep():
    lines = []
    if not os.path.exists(EXP_BUNDLE_DEP_PATH):
        os.makedirs(EXP_BUNDLE_DEP_PATH)

    for file in os.listdir(EXP_BUNDLE_DEP_PATH):
        if file .endswith(BUNDLE_DEP_EXT_NAME):
            fc = open(path.join(EXP_BUNDLE_DEP_PATH, file), 'rb')
            if not fc:
                raise Exception, "can not open file: " + file
            fl = fc.readlines()
            lines.extend(fl)
    fo = open(path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE), 'wb')
    if not fo:
        raise Exception, "can not open file: " + EXP_BUNDLE_DEP_FILE
    fo.writelines(lines)
    fo.close()

def gen_asset_dep():
    lines = []
    if not os.path.exists(EXP_ASSET_DEP_PATH):
        os.makedirs(EXP_ASSET_DEP_PATH)

    for file in os.listdir(EXP_ASSET_DEP_PATH):
        if file .endswith(ASSET_DEP_EXT_NAME):
            fc = open(path.join(EXP_ASSET_DEP_PATH, file), 'rb')
            if not fc:
                raise Exception, "can not open file: " + file
            fl = fc.readlines()
            lines.extend(fl)
    fo = open(path.join(EXP_ASSET_DEP_PATH, EXP_ASSET_DEP_FILE), 'wb')
    if not fo:
        raise Exception, "can not open file: " + EXP_ASSET_DEP_FILE
    fo.writelines(lines)
    fo.close()

def gen_audio_info():
    lines = []
    if not os.path.exists(EXP_AUDIO_INFO_PATH):
        os.makedirs(EXP_AUDIO_INFO_PATH)

    for file in os.listdir(EXP_AUDIO_INFO_PATH):
        if file.endswith(AUDIO_INFO_EXT_NAME):
            fc = open(path.join(EXP_AUDIO_INFO_PATH, file), 'rb')
            if not fc:
                raise Exception, "can not open file: " + file
            fl = fc.readlines()
            lines.extend(fl)
    fo = open(path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE), 'wb')
    if not fo:
        raise Exception, "can not open file: " + EXP_AUDIO_INFO_FILE
    fo.writelines(lines)
    fo.close()

def md5sum(filename):
    md5 = hashlib.md5()
    with open(filename,'rb') as f: 
        for chunk in iter(lambda: f.read(128 * md5.block_size), b''): 
             md5.update(chunk)
    return md5.hexdigest()

# def gen_swf_checksum():
#     lines = []
#     for file in os.listdir(EXP_FLASH_PATH):
#         md5 = md5sum(path.join(EXP_FLASH_PATH, file))
#         lines.append(file + ',' + md5 + ',' + 'false' + '\n')

#     mf = open(path.join(EXP_FLASH_PATH, FLASH_INFO_FILE), 'wb')
#     if not mf:
#         raise Exception, 'can not open file' + FLASH_INFO_FILE
#     mf.writelines(lines)
#     mf.close()
    
def gen_checksum(platform, subTarget):
    lines = []
    path = os.path.join(EXP_BUNDLE_INFO_PATH, platform, subTarget)
    if not os.path.exists(path):
        return
    for file in os.listdir(path):
        if file.endswith(BUNDLE_INFO_EXT_NAME):
            fc = open(os.path.join(path, file), 'rb')
            if not fc:
                raise Exception, "can not open file: " + file
            fl = fc.readlines()
            lines.extend(fl)

    #cal swf 
    # gen_swf_checksum()
    # mf = open(path.join(EXP_FLASH_PATH, FLASH_INFO_FILE), 'rb')
    # if not mf:
    #     raise Exception, 'can not open file' + FLASH_INFO_FILE
    # lines.extend(mf.readlines())

    #cal bundle map
    md5 = md5sum(os.path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE))
    lines.append(EXP_RES_IDX_FILE + ',' + md5 + ',' + 'false' + '\n')

    #cal res idx
    md5 = md5sum(os.path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE))
    lines.append(EXP_BUNDLE_DEP_FILE + ',' + md5 + ',' + 'false' + '\n')

    #cal audio info
    md5 = md5sum(os.path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE))
    lines.append(EXP_AUDIO_INFO_FILE + ',' + md5 + ',' + 'false' + '\n')

    fo = open(os.path.join(path, EXP_BUNDLE_INFO_FILE), 'wb')
    if not fo:
        raise Exception, "can not open file: " + EXP_BUNDLE_INFO_FILE
    fo.writelines(lines)
    fo.close()

def generate():
    print "gen res idx begin..."
    gen_res_idx()
    print "gen res idx successful"
    
    print "gen bundle dep begin..."
    gen_bundle_dep()
    print "gen bundle dep successful"
    
    print "gen asset dep begin..."
    gen_asset_dep()
    print "gen asset dep successful"

    print "gen audio info begin..."
    gen_audio_info()
    print "gen audio info successful"

    print 'gen check sum begin...'
    gen_checksum(PLATFORM_IPHONE, "")
    gen_checksum(PLATFORM_MAC, "")
    # gen_checksum(PLATFORM_ANDROID, "")
    path = os.path.join(EXP_BUNDLE_INFO_PATH, PLATFORM_ANDROID)
    if os.path.exists(path):
        for dir in os.listdir(path):
            if os.path.isdir(os.path.join(path, dir)):
                gen_checksum(PLATFORM_ANDROID, dir)
    print 'gen check sum successful'
    
if __name__ == '__main__':
    generate()
    #gen_bundle_dep()
