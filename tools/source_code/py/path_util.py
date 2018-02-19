# -*- coding:utf-8 -*-

"""copy necessary from the res project to the src project
"""
import os
import sys
import shutil
import stat

def force_del(file):
    if not os.path.exists(file):
        return

    if os.path.isfile(file):
        os.chmod(file, stat.S_IREAD | stat.S_IWRITE)
        os.remove(file)
    else:
        shutil.rmtree(file)
    
def clear_folder(folder):
    if not os.path.exists(folder):
        return
    res_list = os.listdir(folder)
    for file in res_list:
        force_del(os.path.join(folder, file))

def clear_folder_type(folder, type):
    if not os.path.exists(folder):
        return
    res_list = os.listdir(folder)
    for file in res_list:
        if os.path.splitext(file)[1] == type:
            force_del(os.path.join(folder, file))

def clear_folder_exclude_type(folder, type):
    if not os.path.exists(folder):
        return
    res_list = os.listdir(folder)
    for file in res_list:
        if os.path.splitext(file)[1] != type:
            force_del(os.path.join(folder, file))

def copy_folder(src, dst):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)
    src_list = os.listdir(src)
    for file in src_list:
        shutil.copy(os.path.join(src, file), dst)

def copy_folder_type(src, dst, type):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)
    src_list = os.listdir(src)
    for file in src_list:
        if os.path.splitext(file)[1] == type:
            shutil.copy(os.path.join(src, file), dst)

def copy_folder_replace_type(src, dst, srcType, dstType):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)
    src_list = os.listdir(src)
    for file in src_list:
        if os.path.splitext(file)[1] == srcType:
            shutil.copy(os.path.join(src, file), dst)
            srcName = os.path.join(dst) + "/" + file
            dstName = os.path.join(dst) + "/" + os.path.splitext(file)[0] + dstType
            os.rename(srcName, dstName)

def copy_folder_exclude_type(src, dst, type):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)
    src_list = os.listdir(src)
    for file in src_list:
        if os.path.splitext(file)[1] != type:
            shutil.copy(os.path.join(src, file), dst)

def recurse_clear_folder_type(folder, type, exclude_name_list):
    if not os.path.exists(folder):
        return
    if not os.path.isdir(folder):
        return

    for i in os.listdir(folder):
        if i in exclude_name_list:
            continue
        dir = os.path.join(folder, i)
        if os.path.isdir(dir):
            recurse_clear_folder_type(dir, type, exclude_name_list)

        name, ext = os.path.splitext(i)
        if ext == type:
            force_del(dir)

def recurse_clear_folder(folder, exclude_name_list):
    if not os.path.exists(folder):
        return
    if not os.path.isdir(folder):
        return

    for i in os.listdir(folder):
        if i in exclude_name_list:
            continue
        dir = os.path.join(folder, i)
        if os.path.isdir(dir):
            recurse_clear_folder(dir, exclude_name_list)

        force_del(dir)

def recurse_copy_folder_type(src, dst, type, exclude_name_list):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)

    for i in os.listdir(src):
        if i in exclude_name_list:
            continue
        src_dir = os.path.join(src, i)
        dst_dir = os.path.join(dst, i)
        if os.path.isdir(src_dir):
            recurse_copy_folder_type(src_dir, dst_dir, type, exclude_name_list)
        else:
            name, ext = os.path.splitext(i)
            if ext == type:
                shutil.copy(src_dir, dst)

def recurse_copy_folder(src, dst, exclude_name_list):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)

    for i in os.listdir(src):
        if i in exclude_name_list:
            continue
        src_dir = os.path.join(src, i)
        dst_dir = os.path.join(dst, i)
        if os.path.isdir(src_dir):
            recurse_copy_folder(src_dir, dst_dir, exclude_name_list)
        else:
            shutil.copy(src_dir, dst)
            
def recurse_copy_folder_type_to_same_dst(src, dst, type, exclude_name_list):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)

    for i in os.listdir(src):
        if i in exclude_name_list:
            continue
        src_dir = os.path.join(src, i)
        if os.path.isdir(src_dir):
            recurse_copy_folder_type_to_same_dst(src_dir, dst, type, exclude_name_list)
        else:
            name, ext = os.path.splitext(i)
            if ext == type:
                shutil.copy(src_dir, dst)

def recurse_copy_folder_to_same_dst(src, dst, exclude_name_list):
    if not os.path.exists(src):
        return
    if not os.path.exists(dst):
        os.makedirs(dst)

    for i in os.listdir(src):
        if i in exclude_name_list:
            continue
        src_dir = os.path.join(src, i)
        if os.path.isdir(src_dir):
            recurse_copy_folder_to_same_dst(src_dir, dst, exclude_name_list)
        else:
            shutil.copy(src_dir, dst)

def recurse_del_svn(path):
    if not os.path.exists(path):
        return
    for i in os.listdir(path):
        dir = os.path.join(path, i)
        if os.path.isdir(dir):
            if os.path.splitext(dir)[1] == '.svn':
                recurse_rmdir(dir)
            else:
                recurse_del_svn(dir)

def recurse_rmdir(dir):
    if not os.path.exists(dir):
        return
    for file in os.listdir(dir):
        fileurl = os.path.join(dir, file)
        if os.path.isfile(fileurl):
            force_del(fileurl)
        else:
            recurse_rmdir(fileurl)
    force_del(dir)

def recurse_rmdir(dir, exclude_name_list):
    if not os.path.exists(dir):
        return
    for file in os.listdir(dir):
        if file in exclude_name_list:
            continue
        fileurl = os.path.join(dir, file)
        if os.path.isfile(fileurl):
            force_del(fileurl)
        else:
            recurse_rmdir(fileurl, exclude_name_list)
    force_del(dir)
    
def comment_line_in_file(file_name, string_in_line):
    if not os.path.exists(file_name):
        return
        
    file = open(file_name, "r")
    lines = file.readlines()
    file.close()
    write_lines = []
    for line in lines:
        write_lines.append(line.replace(string_in_line, '{;}//' + string_in_line))
        
    file = open(file_name, "w")
    file.writelines(write_lines)
    
def replace_str_in_file(file_name, old_str, new_str):
    if not os.path.exists(file_name):
        return

    file = open(file_name, "r")
    lines = file.readlines()
    file.close()
    write_lines = []
    for line in lines:
        write_lines.append(line.replace(old_str, new_str))
        
    file = open(file_name, "w")
    file.writelines(write_lines)
