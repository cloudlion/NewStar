# -*- coding:utf-8 -*-
"""path config
"""

import os
import platform

#gobal path config
CWD = os.path.normpath(os.path.abspath(os.path.dirname(os.path.realpath(__file__))))
PLATFORM = platform.system()
APP_NAME = 'aok'

ROOT_PATH = os.path.normpath(os.path.abspath(os.path.join(CWD, '../../../')))
SRC_PATH = os.path.join(ROOT_PATH, 'client')
ASSET_PATH = SRC_PATH
#GO_PATH = os.path.join(ROOT_PATH, 'mad_max_be/mad_golang')
#FLASH_PATH = os.path.join(ROOT_PATH, 'mad_flash')
CONF_PATH = os.path.join(ROOT_PATH, 'mad_max_design/mad_conf')
RES_PATH = os.path.join(ROOT_PATH, 'mad_max_art/mad_res')
TOOL_PATH = os.path.join(ROOT_PATH, 'tools')
PROTO_PATH = os.path.join(ROOT_PATH, 'proto')
PROVISION_PATH = os.path.join(ROOT_PATH, 'tools/provision')
OTA_PATH = os.path.join(ROOT_PATH, '../../adam_ota')

#temp folder
RELEASE_PATH = os.path.join(ROOT_PATH, 'release')
RELEASE_APP_PATH = os.path.join(ROOT_PATH, 'ios/build')
RELEASE_IPA_PATH = os.path.join(RELEASE_PATH, 'app')
EXP_PATH = os.path.join(ROOT_PATH, 'mad_bundle')

RES_PREFAB_PATH = os.path.join(RES_PATH, 'unity_prefab')
RES_PREFAB_2D_PATH = os.path.join(RES_PATH, 'unity_prefab_2d')
RES_RAW_PATH = os.path.join(RES_PATH, 'unity_raw')
RES_NGUI_PATH = os.path.join(RES_PATH, 'unity_ngui')
RES_SCENE_PATH = os.path.join(RES_PATH, 'unity_scene')
RES_SCENE_2D_PATH = os.path.join(RES_PATH, 'unity_scene_2d')
RES_GAMEDATA_PATH = os.path.join(RES_PATH, 'unity_gamedata')
RES_AUDIO_PATH = os.path.join(RES_PATH, 'unity_audio')

EXP_RES_IDX_PATH = os.path.join(EXP_PATH, 'res_idx')
EXP_BUNDLE_DEP_PATH = os.path.join(EXP_PATH, 'bundle_dep')
EXP_ASSET_DEP_PATH = os.path.join(EXP_PATH, 'asset_dep')
EXP_BUNDLE_INFO_PATH = os.path.join(EXP_PATH, 'bundle_info')
EXP_BUNDLE_PATH = os.path.join(EXP_PATH, 'bundle')
EXP_SCRIPT_PATH = os.path.join(EXP_PATH, 'script')
EXP_SHADER_PATH = os.path.join(EXP_PATH, 'shader')
#EXP_FLASH_PATH = os.path.join(EXP_PATH, 'flash')
EXP_AUDIO_INFO_PATH = os.path.join(EXP_PATH, 'audio_info')

EXP_BUNDLE_MAC_PATH = os.path.join(EXP_BUNDLE_PATH, 'mac')
EXP_BUNDLE_IOS_PATH = os.path.join(EXP_BUNDLE_PATH, 'iphone')
EXP_BUNDLE_ANDROID_PATH = os.path.join(EXP_BUNDLE_PATH, 'android')

SRC_PROJ_PATH = os.path.join(SRC_PATH, 'Assets')
GOOGLE_PLAY_DIR = os.path.join(SRC_PROJ_PATH, 'GooglePlayGames')
GOOGLE_PLAY_DIR_META = os.path.join(SRC_PROJ_PATH, 'GooglePlayGames.meta')
GPGS_FILE = os.path.join(SRC_PROJ_PATH, 'GPGSIds.cs')
GPGS_FILE_META = os.path.join(SRC_PROJ_PATH, 'GPGSIds.cs.meta')
PLAY_SERVICES_DIR = os.path.join(SRC_PROJ_PATH, 'PlayServicesResolver')
PLAY_SERVICES_DIR_META = os.path.join(SRC_PROJ_PATH, 'PlayServicesResolver.meta')
ANDROID_PRO_DIR = os.path.join(SRC_PROJ_PATH, 'Plugins/Android')
ANDROID_PRO_META = os.path.join(SRC_PROJ_PATH, 'Plugins/Android.meta')
WINDOWS_PLUGIN_DIR = os.path.join(SRC_PROJ_PATH, 'Plugins/Windows')
#GO_SRC_PATH = os.path.join(GO_PATH, 'src/mad')

PYTHON_TOOL_PATH = os.path.join(TOOL_PATH, 'py')
MAKO_PATH = os.path.join(PYTHON_TOOL_PATH, 'mako')

#res idx
BUNDLE_EXT_NAME = '.bundle'
RES_IDX_EXT_NAME = '.res_idx'
BUNDLE_DEP_EXT_NAME = '.dep_idx'
ASSET_DEP_EXT_NAME = '.asset_dep_idx'
BUNDLE_INFO_EXT_NAME = '.bd_info'
AUDIO_INFO_EXT_NAME = '.audio_idx'
#FLASH_INFO_FILE = 'flash.swf_info'

#RES_IDX_MAP_MAKO = 'ResMapIdx.mako'
EXP_RES_IDX_FILE = 'asset_map.txt'
EXP_BUNDLE_DEP_FILE = 'bundle_map.txt'
EXP_BUNDLE_INFO_FILE = 'bundle_info.txt'
EXP_ASSET_DEP_FILE = 'asset_dep.txt'
EXP_AUDIO_INFO_FILE = 'audio_info.txt'

PLATFORM_IPHONE = 'iphone'
PLATFORM_ANDROID = 'android'
PLATFORM_MAC = 'mac'

#game conf
GAME_CONF_MAKO_CS = 'Config_cs.mako'
GAME_CONF_FACT_MAKO_CS = 'ConfFact_cs.mako'
# GAME_CONF_MAKO_GO = 'Config_go.mako'
# GAME_CONF_FACT_MAKO_GO = 'ConfFact_go.mako'
GAME_CONF_PCK_FLAG = 'cs'
SRC_GAME_CONF_PATH = os.path.join(CONF_PATH, '')
EXP_GAME_CONF_CS_PATH = os.path.join(EXP_PATH, 'exp_conf_code_cs')
#EXP_GAME_CONF_GO_PATH = os.path.join(EXP_PATH, 'exp_conf_code_go')
EXP_GAME_CONF_BIN_PATH = os.path.join(EXP_PATH, 'game_conf_bin')
EXP_GAME_CONF_CSV_PATH = os.path.join(EXP_PATH, 'game_conf_csv')

#proto
PROTO_GENERATOR = 'ClientGen/genproto'
PROTO_CONF_FILE = 'ClientGen/proto.toml'
PROTO_FLAG = 'cs'
#PROTO_SRC_PATH = PROTO_PATH
#PROTO_CS_PATH = os.path.join(PROTO_PATH, 'cs')
#PROTO_GO_PATH = os.path.join(PROTO_PATH, 'go')
#PROTO_GEN_PATH = os.path.join(TOOL_PATH, 'ProtoGen')
#PROTO_GEN = 'protogen.exe'
#PROTO_ID_FILE = os.path.join(PROTO_GO_PATH, 'protoid.go')
#PROTO_ROUTE_FILE = os.path.join(PROTO_GO_PATH, 'routemap.go')

#copy res path config
DST_SCRIPT_PATH = os.path.join(SRC_PROJ_PATH, 'Script/res_script')
DST_SHADER_PATH = os.path.join(SRC_PROJ_PATH, 'Resources/res_shader')
DST_GAME_CONF_CS_PATH = os.path.join(SRC_PATH, 'Assets/Script/gamedata')
#DST_GAME_CONF_GO_PATH = os.path.join(GO_SRC_PATH, 'gamedata')
#DST_GAME_CONF_BIN_GO_PATH = os.path.join(GO_PATH, 'gamedata')
#DST_PROTO_GO_PATH = os.path.join(GO_SRC_PATH, 'proto')
DST_PROTO_CS_PATH = os.path.join(SRC_PROJ_PATH, 'Script/proto')

DST_RES_PATH = os.path.join(RELEASE_PATH, 'res')
DST_RES_MAC_PATH = os.path.join(DST_RES_PATH, 'mac')
DST_RES_IOS_PATH = os.path.join(DST_RES_PATH, 'iphone')
DST_RES_ANDROID_PATH = os.path.join(DST_RES_PATH, 'android')

ASTYLE_PATH = os.path.join(ROOT_PATH, 'tools/source_code/csharpformat/astyle/build/mac/bin/astyle')
ASTYLE_FORMAT_PATH = os.path.join(ROOT_PATH, 'tools/source_code/csharpformat/csharpformat.astylerc')

IOS_PROJ = os.path.join(ROOT_PATH, 'ios/Unity-iPhone.xcodeproj')
IOS_PUBLISH_PROJ = os.path.join(ROOT_PATH, 'ios_publish/Unity-iPhone.xcodeproj')
IOS_DEBUG_APP = os.path.join(RELEASE_APP_PATH, 'Debug-iphoneos/aok.app')
IOS_RELEASE_APP = os.path.join(RELEASE_APP_PATH, 'Release-iphoneos/aok.xcarchive')
IOS_TARGET = '\"Unity-iPhone\"'
IOS_SDK = 'iphoneos'
IOS_DEBUG_IPA = 'aok_debug.ipa' #os.path.join(RELEASE_APP_PATH, 'aok_debug.ipa')
IOS_RELEASE_IPA = 'aok_release.ipa'#os.path.join(RELEASE_APP_PATH, 'Release-iphoneos/aok_release.ipa')
IOS_SIGN_CERT = 'iPhone Developer: Han Liang'
PROVISION_DEV = os.path.join(PROVISION_PATH, 'aok_dev.mobileprovision')

# TESTFLIGHT_UPLOAD_URL = 'http://testflightapp.com/api/builds.json'
# TESTFLIGHT_API_TOKEN = '3c0ec25f9ce0f7e6daa080c7c522b573_MjA1OTQxMDIwMTQtMDgtMjQgMjM6MzE6MDAuODg4NDk5'
# TESTFLIGHT_TEAM_TOKEN = '2e2db0a33ac3dd366889057889367aab_NDM4NzQ3MjAxNC0wOS0yNCAyMzozNTo1OC4xMzE2MTI'
# TESTFLIGHT_DISTRIBUTION_LIST = 'hijack_internal'
CODESIGN = '/usr/bin/codesign'
KEYCHAIN = '~/Library/Keychains/login.keychain'
P12_FILE = os.path.join(PROVISION_PATH, 'Certificates-aok.p12')
P12_PWD = '123'

PUBLISH_P12 = os.path.join(PROVISION_PATH, 'netherfire/distribution_p12.p12')
PUBLISH_P12_PWD = 'netherfire_goc'

OS_ACCOUNT = 'hijack'
OS_PWD = 'Linkage1234'

SRC_VERSION_FILE = os.path.join(SRC_PROJ_PATH, 'Script/Game/GameVersion.cs')
SRC_VERSION_VAR = 'string version ='
SRC_PLATEFORM_VAR = 'string plateform ='
SRC_BRANCH_VAR = 'string branch ='