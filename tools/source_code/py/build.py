#!/usr/bin/env python
# -*- coding:utf-8 -*-

import os
import re
import sys
import getopt
import subprocess
import time
import except_handler
from path_util import *
from config import *

import pck_config
import gen_res_idx
import copy_res
import pack_proto
import parse_proto

def parse_step(step):
    sts = []
    if not step.strip().lower():
        sts = ['unity', 'runtime', 'src']
    else:
        sts = step.strip().lower().split('|')
    return sts
    
def parse_platform(platform):
    pm = []
    if not platform.strip().lower():
        pm = ['android', 'iphone']
    else:
        pm = platform.strip().lower().split('|')
    return pm

def parse_cfg(cfg):
    l = []
    if not cfg.strip().lower():
        l = ['debug', 'release']
    else:
        l = cfg.strip().lower().split('|')
    return l

def delete_file_folder(src):
    '''delete files and folders'''
    if os.path.isfile(src):
        try:
            os.remove(src)
        except:
            pass
    elif os.path.isdir(src):
        for item in os.listdir(src):
            itemsrc=os.path.join(src,item)
            delete_file_folder(itemsrc) 
        try:
            os.rmdir(src)
        except:
            pass

def build(step, platform, cfg, ver, tunnel, branch):
    exphook = sys.excepthook
    sys.excepthook = except_handler._excepthook
    delete_file_folder(GOOGLE_PLAY_DIR)
    delete_file_folder(GOOGLE_PLAY_DIR_META)
    delete_file_folder(GPGS_FILE)
    delete_file_folder(GPGS_FILE_META)
    delete_file_folder(PLAY_SERVICES_DIR)
    delete_file_folder(PLAY_SERVICES_DIR_META)
    delete_file_folder(ANDROID_PRO_DIR)
    delete_file_folder(ANDROID_PRO_META)
    delete_file_folder(WINDOWS_PLUGIN_DIR)
    if not ver:
        ver = '0.0'
    
    sts = parse_step(step)
    if 'runtime' in sts:
        build_runtime_res()
        
    pms = parse_platform(platform)

    if 'assets' in sts:
            clear_ota() 
    for pm in pms:
        update_src_version(ver, pm, branch)
        if 'assets' in sts:
            build_unity_res(platform)
   
        if 'src' in sts:
            build_unity_src(pm, cfg, ver, tunnel)
            build_app(pm, cfg, ver, tunnel)

    if 'assets' in sts:
            commit_ota()
    sys.excepthook = exphook

def build_android_app(cfg, ver):
    cfgs = parse_cfg(cfg)
    if 'debug' in cfgs:
        pass
    if 'release' in cfgs:
        pass

def build_mac_app(cfg, ver):
    cfgs = parse_cfg(cfg)
    if 'debug' in cfgs:
        pass
    if 'release' in cfgs:
        pass

def build_iphone_app(cfg, ver, tunnel):
    cfgs = parse_cfg(cfg)
    
    proj_path = IOS_PROJ
    scheme = 'Unity-iPhone'
    plist_path = os.path.join(os.path.join(ROOT_PATH, 'tools/source_code/exportOptions'), 'build_adhoc.plist')
    if 'submit' in cfgs:
        plist_path = os.path.join(os.path.join(ROOT_PATH, 'tools/source_code/exportOptions'), 'build_submit.plist')
        
    if 'internal' in tunnel:
        p12_pwd = P12_PWD
        p12 = P12_FILE
        target = 'Unity-iPhone'
    else:
        p12_pwd = PUBLISH_P12_PWD
        p12 = PUBLISH_P12
        target = 'submit'
        proj_path = IOS_PUBLISH_PROJ

    os.system('security unlock-keychain -p ' + OS_PWD + ' ' + KEYCHAIN)
   # os.system('security import ' + p12 + ' -f pkcs12 -P ' + '\"' + p12_pwd + '\"' + ' -k ' + KEYCHAIN + ' -T ' + CODESIGN)
    os.system('security unlock-keychain -p ' + OS_PWD + ' ' + KEYCHAIN)

    now = int(time.time())
    timeArray = time.localtime(now)
    otherStyleTime = time.strftime("%Y%m%d_%H%M%S", timeArray)
    pack_path = os.path.join(RELEASE_APP_PATH, ver)
    
    if 'debug' in cfgs:
        pack_name = APP_NAME + '_debug_' + otherStyleTime
    elif 'release' in cfgs:
        pack_name = APP_NAME + '_' + otherStyleTime
    elif 'submit' in cfgs:
        pack_name = APP_NAME + '_submit_' + otherStyleTime
        
    archive_path = os.path.join(pack_path, pack_name + '.xcarchive')
    ipa_path = os.path.join(pack_path, pack_name)
        
    build_cmd = 'xcodebuild -list ' \
    + ' -project ' +proj_path
    os.system(build_cmd)
        
    build_cmd = 'xcodebuild' \
    + ' -project ' + proj_path \
    + ' -configuration ' + 'Release' \
    + ' -arch ' + 'armv7 ' \
    + ' -arch ' + 'arm64 ' \
    + ' -sdk ' + IOS_SDK \
    + ' -scheme ' + scheme \
    + ' clean ' \
    + ' archive ' \
    + ' -archivePath ' + archive_path
    os.system(build_cmd)

            
    pck_cmd = 'xcodebuild' \
    + ' -exportArchive '  \
    + ' -archivePath ' + archive_path \
    + ' -exportPath ' + ipa_path \
    + ' -exportOptionsPlist ' + plist_path
    os.system(pck_cmd)
    os.rename(ipa_path + '/Unity-iPhone.ipa',ipa_path + '/' + pack_name + '.ipa')

   
def build_app(platform, cfg, ver, tunnel):
    pms = parse_platform(platform)

    if 'ios' in pms:
        build_iphone_app(cfg, ver, tunnel)
    if 'android' in pms:
        build_android_app(cfg, ver)
    if 'mac' in pms:
        build_mac_app(cfg, ver)

def update_src_version(ver, plateform, branch):
    f = open(SRC_VERSION_FILE, 'rb')
    if not f:
        raise Exception, 'can not open src version file: ' + SRC_VERSION_FILE

    lines = f.readlines()
    f.close()
    
    olines = []
    for l in lines:
        if SRC_VERSION_VAR in l:
            elems = l.split('=')
            if len(elems) < 2:
                continue
            else:
                olines.append(elems[0].rstrip() + ' = ' + '\"' + ver + '\"' + ';\n')
        elif SRC_PLATEFORM_VAR in l:
            elems = l.split('=')
            if len(elems) < 2:
                continue
            else:
                olines.append(elems[0].rstrip() + ' = ' + '\"' + plateform + '\"' + ';\n')
        elif SRC_BRANCH_VAR in l:
            elems = l.split('=')
            if len(elems) < 2:
                olines.append(l)
            elif branch != 'QA' and branch != 'master':
                olines.append(elems[0].rstrip() + ' = ' + '\"' + branch + '\"' + ';\n')
            else:
                olines.append(l)
        else:
            olines.append(l)
    of = open(SRC_VERSION_FILE, 'wb')
    of.writelines(olines)
    of.close()

    
def build_unity_src(platform, cfg, ver, tunnel):
    
    
    unity_app = ""
    if PLATFORM == "Windows":
        unity_app = "Unity.exe "
    elif PLATFORM == "Darwin":
        unity_app = "/Applications/Unity/Unity.app/Contents/MacOS/Unity "
    log_file = os.path.join(ROOT_PATH, "unity_src.log")
    cmd = unity_app + " -batchmode -quit "
    cmd += " -logFile " + log_file
    cmd += " -projectPath " + SRC_PATH
    cmd += " -executeMethod Build.BuildProject "
    
    print "pck src begin..."
    os.environ['PLATFORM'] = platform
    os.environ['CONFIG'] = cfg
    os.environ['VERSION'] = ver
    os.environ['TUNNEL'] = tunnel
    os.system(cmd)

    file = open(log_file, 'r')
    error = False
    for line in file:
        if "BuildException" in line or 'Fatal error' in line:
            print "error: " + line
            error = True

    if error:
        raise Exception, "There is an error occured! please check"

    print "pck src end"

def build_unity_res(platform):
    unity_app = ""
    if PLATFORM == "Windows":
        unity_app = "Unity.exe "
    elif PLATFORM == "Darwin":
        unity_app = "/Applications/Unity/Unity.app/Contents/MacOS/Unity "
    log_file = os.path.join(ROOT_PATH, "tmp")
    cmd = unity_app + " -batchmode -quit "
    #cmd += " -logFile " + log_file
    cmd += " -projectPath "

    os.system("rm -f *.log")
    
    param = ""
    task_list = {
                    "unity_asset.log" : [ASSET_PATH, "CreateAssetBundles.BuildAllAssetBundles"],
                }

    pckPids = []
    os.environ['PLATFORM'] = platform
    
    for i in task_list:
        pckPids.append(subprocess.Popen(cmd + task_list[i][0] + " -logFile " + i + " -executeMethod " + task_list[i][1], shell=True, close_fds=True, env=os.environ))

    print ', '.join(i.replace(".log","") for i in task_list.keys()) + " packing started"
    
    wait_for_tasks(pckPids)
    # detect error
    error = False
    for i in task_list:
        path = os.path.join(ROOT_PATH, i)
        if not os.path.exists(path):
            continue

        file = open(path, 'r')

        for line in file:
            if "Exception" in line:
                print "error: " + line
                error = True

    if error:
        raise Exception, "There is an error occured! please check"
def clear_ota():
    os.popen('sh ../clearOTA.sh ' + OTA_PATH)

def commit_ota():
    os.popen('sh ../commitOTA.sh ' + OTA_PATH)
    
def wait_for_tasks(pckPids):
    
    moreWait = True
    while moreWait:
        moreWait = False
        for p in pckPids:
            p.poll()
            if p.returncode == None:
                moreWait = True

        if moreWait:
            time.sleep(5)

def build_runtime_res():
#  print "gen res idx begin..."
#   gen_res_idx.generate()
#    print "gen res idx success"

    print 'pack proto begin...'
    pack_proto.gen_cs()
    #pack_proto.gen_go(PROTO_SRC_PAdfdTH, PROTO_GO_PATH)
    #parse_proto.parse(PROTO_SRC_PATH, PROTO_ID_FILE, PROTO_ROUTE_FILE)
    print 'pack proto success'

#    print "copy res begin..."
#    copy_res.copy_res()
#    print "copy res success"


if __name__ == '__main__':
    step = 'src'
    platform = 'ios'
    cfg = 'debug'
    ver = '0.01'
    tun = 'internal'
    branch = 'QA'
    try:
        opts, args = getopt.getopt(sys.argv[1:], 's:p:c:v:t:b:', ['step=', 'platform=', 'config=', 'version=', 'tunnel=', 'branch='])
        for opt, arg in opts:
            if opt in ('-s', '--step'):
                step = arg.replace(',', '|')
            elif opt in ('-p', '--platform'):
                platform = arg.replace(',', '|')
            elif opt in ('-c', '--config'):
                cfg = arg.replace(',', '|')
            elif opt in ('-v', '--version'):
                ver = arg
            elif opt in ('-t', '--tunnel'):
                tun = arg
            elif opt in ('-b', '--branch'):
                branch = arg
    except getopt.GetoptError:
        print """usage: build.py -s assets|runtime|src 
        -p ios|android
        -c debug|release
        -v version number
        -t tunnel internal|publish
        -b branch QA"""
        os.exit(2)
    print opts
    print args
    build(step, platform, cfg, ver, tun, branch)
 #   pack_proto.gen_cs()