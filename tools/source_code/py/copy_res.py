#!/usr/bin/env python
# -*- coding:utf-8 -*-

import os
import shutil
from config import *
from path_util import *

def copy_res():
    dir = os.getcwd()
    os.chdir(ROOT_PATH)

    if not os.path.exists(DST_RES_MAC_PATH):
        os.makedirs(DST_RES_MAC_PATH)

    if not os.path.exists(DST_RES_IOS_PATH):
        os.makedirs(DST_RES_IOS_PATH)

    if not os.path.exists(DST_RES_ANDROID_PATH):
        os.makedirs(DST_RES_ANDROID_PATH)

    #copy bundle & scene
    # create all subfolders
    recurse_copy_folder(EXP_BUNDLE_MAC_PATH, DST_RES_MAC_PATH, [])
    recurse_copy_folder(EXP_BUNDLE_IOS_PATH, DST_RES_IOS_PATH, [])
    recurse_copy_folder(EXP_BUNDLE_ANDROID_PATH, DST_RES_ANDROID_PATH, [])
    # recurse_copy_folder_to_same_dst(EXP_BUNDLE_ANDROID_PATH, DST_RES_ANDROID_PATH, [])

    #copy info data
    shutil.copy(os.path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE), DST_RES_MAC_PATH)
    shutil.copy(os.path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE), DST_RES_IOS_PATH)
    for dir in os.listdir(DST_RES_ANDROID_PATH):
        if os.path.isdir(os.path.join(DST_RES_ANDROID_PATH, dir)):
            shutil.copy(os.path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE), os.path.join(DST_RES_ANDROID_PATH, dir))
    # shutil.copy(os.path.join(EXP_RES_IDX_PATH, EXP_RES_IDX_FILE), DST_RES_ANDROID_PATH)
    
    shutil.copy(os.path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE), DST_RES_MAC_PATH)
    shutil.copy(os.path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE), DST_RES_IOS_PATH)
    for dir in os.listdir(DST_RES_ANDROID_PATH):
        if os.path.isdir(os.path.join(DST_RES_ANDROID_PATH, dir)):
            shutil.copy(os.path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE), os.path.join(DST_RES_ANDROID_PATH, dir))
    # shutil.copy(os.path.join(EXP_BUNDLE_DEP_PATH, EXP_BUNDLE_DEP_FILE), DST_RES_ANDROID_PATH)

    srcf = os.path.join(os.path.join(EXP_BUNDLE_INFO_PATH, PLATFORM_MAC),
                        EXP_BUNDLE_INFO_FILE)
    if os.path.exists(srcf):
        shutil.copy(srcf, DST_RES_MAC_PATH)
    srcf = os.path.join(os.path.join(EXP_BUNDLE_INFO_PATH, PLATFORM_IPHONE), 
                        EXP_BUNDLE_INFO_FILE)
    if os.path.exists(srcf):
        shutil.copy(srcf, DST_RES_IOS_PATH)
    for dir in os.listdir(DST_RES_ANDROID_PATH):
        if os.path.isdir(os.path.join(DST_RES_ANDROID_PATH, dir)):
            srcf = os.path.join(EXP_BUNDLE_INFO_PATH, PLATFORM_ANDROID, dir, EXP_BUNDLE_INFO_FILE)
            if os.path.exists(srcf):
                shutil.copy(srcf, os.path.join(DST_RES_ANDROID_PATH, dir))
    # shutil.copy(os.path.join(os.path.join(EXP_BUNDLE_INFO_PATH, PLATFORM_ANDROID),
    #                          EXP_BUNDLE_INFO_FILE),
    #             DST_RES_ANDROID_PATH)

    shutil.copy(os.path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE), DST_RES_MAC_PATH)
    shutil.copy(os.path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE), DST_RES_IOS_PATH)
    for dir in os.listdir(DST_RES_ANDROID_PATH):
        if os.path.isdir(os.path.join(DST_RES_ANDROID_PATH, dir)):
            shutil.copy(os.path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE), os.path.join(DST_RES_ANDROID_PATH, dir))
    # shutil.copy(os.path.join(EXP_AUDIO_INFO_PATH, EXP_AUDIO_INFO_FILE), DST_RES_ANDROID_PATH)

    #copy script
    recurse_clear_folder(DST_SCRIPT_PATH, [])
    recurse_copy_folder_to_same_dst(EXP_SCRIPT_PATH, DST_SCRIPT_PATH, [])
    
    #copy shader
    recurse_clear_folder(DST_SHADER_PATH, [])
    recurse_copy_folder_to_same_dst(EXP_SHADER_PATH, DST_SHADER_PATH, [])
    
    #copy game conf code
    clear_folder_type(DST_GAME_CONF_CS_PATH, '.cs')
    copy_folder_type(EXP_GAME_CONF_CS_PATH, DST_GAME_CONF_CS_PATH, '.cs')
    
    # clear_folder_type(DST_GAME_CONF_GO_PATH, '.go')
    # copy_folder_type(EXP_GAME_CONF_GO_PATH, DST_GAME_CONF_GO_PATH, '.go')

    # clear_folder_type(DST_GAME_CONF_BIN_GO_PATH, '.bytes')
    # copy_folder_type(EXP_GAME_CONF_BIN_PATH, DST_GAME_CONF_BIN_GO_PATH, '.bytes')

    #copy proto
    #clear_folder_type(DST_PROTO_GO_PATH, '.go')
    #copy_folder_type(PROTO_GO_PATH, DST_PROTO_GO_PATH, '.go')

    #recurse_clear_folder(DST_PROTO_CS_PATH, ['.cs'])
    #copy_folder_type(PROTO_CS_PATH, DST_PROTO_CS_PATH, '.cs')

if __name__ == '__main__':
    print "copy res begin..."
    copy_res()
    print "copy res successful"
