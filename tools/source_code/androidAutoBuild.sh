#!/bin/sh

#example ./androidAutoBuild.sh 1.0.0 debug/release
# projectPath="/Users/cary/Documents/WorkAndroidProject/adam/client"
# projectPath="/Users/hliang/Adam/adam_android/client"
projectPath=$(pwd)/../../  #"/Users/linkage/androidProject/adam/client"
unityRootPath="${projectPath}/unity"
copyDestAssetBundlePath="${unityRootPath}/Assets/StreamingAssets/res"
copySourceAssetBundlePath="${unityRootPath}/Assets/AssetBundles/android/stream"
unityAppPath="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
windowsPlugin="${unityRootPath}/Assets/Plugins/Windows"

main()
{
	gitPrepare
        modifyGameVersion $1 $3 $4
	buildAndroidRes
	buildAndroidApk $1 $2
    
}

gitPrepare()
{
	echo -e "\n git clean"
	cd ${unityRootPath}
	git checkout . -f
	git clean -fd
	if [ $? -ne 0 ];then
        echo -e "\nGit clean error! Error code:$?"
   	 	exit
   	fi
    git pull
    if [ $? -ne 0 ];then
        echo -e "\nGit pull-$1 error! Error code:$?"
        exit
    fi
}

modifyGameVersion()
{
    cd "${projectPath}/tools/source_code"
    python androidVersion.py $1 $2 $3
}

buildAndroidRes()
{
	echo -e "\n start build android res"
	${unityAppPath} -batchmode -projectPath ${unityRootPath} -quit -logFile ~/Documents/buildAndroidApk.log -executeMethod CreateAssetBundles.BuildAndroidBundles
	errorCode=$?
    if [ $errorCode -ne 0 ];then
        echo "\nUnity build res error! Error code:$errorCode"
        exit
    else
        echo "\nUnity build res Success!"
    fi
}

buildAndroidApk()
{
	echo -e "\n start build android apk"
        rm -rf ${windowsPlugin}/
	${unityAppPath} -batchmode -projectPath ${unityRootPath} -quit -logFile ~/Documents/buildAndroidApk.log -executeMethod Build.buildAndroidApk $1 $2 
	errorCode=$?
    if [ $errorCode -ne 0 ];then
        echo "\nUnity build apk error! Error code:$errorCode"
        exit
    else
        echo "\nUnity build apk Success!"
    fi
}

main $1 $2 $3 $4