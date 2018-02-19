#!/usr/bin/env python

import os
import sys
import shutil
from config import *

def copy_config(config, dst_dir):
    shutil.copy2(config, dst_dir)

def main():
    config = os.path.abspath(SRC_PATH + "/ProjectSettings/TagManager.asset")

    copy_config(config, RES_PREFAB_PATH + "/ProjectSettings")
    copy_config(config, RES_PREFAB_2D_PATH + "/ProjectSettings")
    copy_config(config, RES_RAW_PATH + "/ProjectSettings")
    copy_config(config, RES_NGUI_PATH + "/ProjectSettings")
    copy_config(config, RES_SCENE_PATH + "/ProjectSettings")
    copy_config(config, RES_SCENE_2D_PATH + "/ProjectSettings")
    copy_config(config, RES_GAMEDATA_PATH + "/ProjectSettings")

    print "copy TagManager.asset successful"

if __name__ == '__main__':
    main()
