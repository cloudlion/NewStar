import sys
import os
import platform


CWD = os.path.normpath(os.path.abspath(os.path.dirname(os.path.realpath(__file__))))
ROOT_PATH = os.path.normpath(os.path.abspath(os.path.join(CWD, '../../../')))
SRC_PATH = os.path.join(ROOT_PATH, 'client/unity')
SRC_PROJ_PATH = os.path.join(SRC_PATH, 'Assets')
SRC_VERSION_FILE = os.path.join(SRC_PROJ_PATH, 'Script/Game/GameVersion.cs')
SRC_VERSION_VAR = 'string version ='
SRC_PAYMENT_VAR = 'int paymentDebug ='
SRC_PLATEFORM_VAR = 'string plateform ='
SRC_BRANCH_VAR = 'string branch ='

def update_src_version(ver, branch):
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
                olines.append(elems[0].rstrip() + ' = ' + '\"' + 'android' + '\"' + ';\n')
        elif SRC_BRANCH_VAR in l:
            elems = l.split('=')
            if len(elems) < 2:
                continue
            elif branch != 'QA' and branch != 'master':
                olines.append(elems[0].rstrip() + ' = ' + '\"' + branch + '\"' + ';\n')
            else:
                olines.append(l)
        else:
            olines.append(l)
    of = open(SRC_VERSION_FILE, 'wb')
    of.writelines(olines)
    of.close()

def update_payment(ver):
    f = open(SRC_VERSION_FILE, 'rb')
    if not f:
        raise Exception, 'can not open src version file: ' + SRC_PAYMENT_VAR

    lines = f.readlines()
    f.close()
    
    olines = []
    for l in lines:
        if SRC_PAYMENT_VAR in l:
            elems = l.split('=')
            if len(elems) < 2:
                continue
            else:
                olines.append(elems[0].rstrip() + ' = '  + ver  + ';\n')
        else:
            olines.append(l)
    of = open(SRC_VERSION_FILE, 'wb')
    of.writelines(olines)
    of.close()

if __name__ == '__main__':
	version = sys.argv[1]
	payment = sys.argv[2]
	branch   = sys.argv[3]
	update_src_version(version, branch)
	update_payment(payment)
	# try:
 #        opts, args = getopt.getopt(sys.argv[1:], 'v:', ['version='])
 #        for opt, arg in opts:
 #            if opt in ('-v', '--version'):
 #                ver = arg
 #    except getopt.GetoptError:
 #        print """
 #        -s version number"""
 #        os.exit(2)
 # for i in range(len(sys.argv)):
 #    print "a%dge canshu %s" % (i,sys.argv[i])
 # print

