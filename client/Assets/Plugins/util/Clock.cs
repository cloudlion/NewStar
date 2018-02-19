using System;

namespace GameUtil
{
    public class Clock
    {
        //time used for sys time
        private long localSysTime = 0;
        private long frameTime = 0;
        //delta time from last mgr to now
        private long lastFrameTime = 0;

        public void Start()
        {
            localSysTime = SysUtil.TickToMilliSec(System.DateTime.Now.Ticks);
            frameTime = localSysTime;
            lastFrameTime = frameTime - 30;
        }
        public void Shutdown() { }

        public long FrameTime
        {
            get
            {
                return frameTime;
            }
        }
        public long DeltaTime
        {
            get
            {
                return Math.Max(frameTime - lastFrameTime, 0);
            }
        }
        public float DeltaTimeSec
        {
            get
            {
                return this.DeltaTime / 1000.0f;
            }
        }

        public void Update()
        {
            lastFrameTime = frameTime;

            long dt = SysDeltaTime();
            frameTime += dt;
        }

        private long SysDeltaTime()
        {
            long now = SysUtil.TickToMilliSec(System.DateTime.Now.Ticks);
            long dt = now - localSysTime;
            localSysTime = now;

            return dt;
        }
    }
}