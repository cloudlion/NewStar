using UnityEngine;
using System.Collections;

public class Logger  {
	public static void Log(object message)
	{
#if DEBUG
        if (message == null) return;
        Debug.Log (message);
#endif
    }
    public static void LogFormat(string message, params object[] ps)
    {
		#if DEBUG
        Debug.Log(string.Format(message,ps));
		#endif

    }
    public static void LogWarning(object message)
	{
#if DEBUG
        Debug.LogWarning (message);
#endif
    }

    public static void LogError(object message)
	{
#if DEBUG
		Debug.LogError (message);
#endif
    }

    public static void LogException(object message)
	{
		#if DEBUG
        Debug.LogError (message);
		#endif
    }

    public static string LogVariable(object t, string mes = "")
    {

#if DEBUG
        string message = mes + "_______________________________________________ming template " + t.ToString() + "\n";
        try
        {
            if (t == null) return "";
            

            LogSingleClassType(t, ref message, "", "    ", t.ToString());
            Logger.Log(message);
        }
        catch (System.Exception ex)
        {
            Logger.Log("ming log error:" + message);
        }
        return message;
#endif
        return "";
    }

    private static string LogSingleClassType(object t, ref string message, string layer, string tab, string name)
    {
#if DEBUG

        string subLayer = layer + tab;

        message += layer + "Object is " + name + ":\n";
        System.Reflection.PropertyInfo[] ps = t.GetType().GetProperties();
        System.Reflection.FieldInfo[] fs = t.GetType().GetFields();
        message += layer + "(\n";
        if (ps != null)
        {


            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] == null) continue;
                object v = ps[i].GetValue(t, null);
                IEnumerable array = v as IEnumerable;
                if (v == null) continue;
                if (array != null && v.GetType() != typeof(string))
                {
                    LogArrayType(array, ref message, subLayer, tab, ps[i].Name);
                }
                else if (v.GetType().IsValueType || v.GetType() == typeof(string))
                {
                    LogValueType(v, ref message, subLayer, ps[i].Name);
                }
                else
                {
                    LogSingleClassType(ps[i].GetValue(t, null), ref message, subLayer, tab, ps[i].Name);
                }

            }


        }

        if (fs != null)
        {
            for (int i = 0; i < fs.Length; i++)
            {
                if (fs[i] == null) continue;
                object v = fs[i].GetValue(t);
                IEnumerable array = v as IEnumerable;

                if (array != null && v.GetType() != typeof(string))
                {
                    LogArrayType(array, ref message, subLayer, tab, fs[i].Name);
                }
                else if (v.GetType().IsValueType || v.GetType() == typeof(string))
                {
                    LogValueType(v, ref message, subLayer, fs[i].Name);
                }
                else
                {
                    LogSingleClassType(fs[i].GetValue(t), ref message, subLayer, tab, fs[i].Name);
                }

            }

        }
        message += layer + ")\n";
#endif
        return message;

    }

    private static void LogValueType(object t, ref string message, string layer, string name)
    {
#if DEBUG
        message += layer + " [ " + name + " = " + t.ToString() + " ]\n";
#endif
    }
    private static void LogArrayType(IEnumerable arr, ref string message, string layer, string tab,string name)
    {
#if DEBUG
        int num = 0;
        message += layer + " Array is " + name + "\n";
        message += layer + "{\n";
        string sublayer = layer + tab;
        foreach (object v in arr)
        {
            message += sublayer + "index = " + num.ToString() + "\n";

            string nextSubLayer = sublayer + tab;
            message += sublayer + "<\n";
            IEnumerable array = v as IEnumerable;

            if (array != null && v.GetType() != typeof(string))
            {
                LogArrayType(array, ref message, nextSubLayer, tab,name+"["+num+"]");
            }
            else if (v.GetType().IsValueType || v.GetType() == typeof(string))
            {
                LogValueType(v, ref message, nextSubLayer, name + "[" + num + "]");
            }
            else
            {
                LogSingleClassType(v, ref message, nextSubLayer, tab, name + "[" + num + "]");
            }
            message += sublayer + ">\n";
            num++;
        }
        message += "\n"+ layer + "}\n";
#endif
    }


    public static string LogStateMachineParameters(GameObject statemachine)
    {
#if DEBUG


        if (statemachine == null) return string.Empty;
        Animator animator = statemachine.GetComponent<Animator>();
        if (animator == null)
            animator = statemachine.GetComponentInChildren<Animator>();
        if (animator == null) return string.Empty;
        string message = "------------------------------ming statemachine is "+ statemachine.name.ToString() + "\n{\n";
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter p = animator.GetParameter(i);
            
            if (p == null) continue;
            message += "    ";
            switch (p.type)
            {
                case AnimatorControllerParameterType.Bool:
                    message += "[" + p.name.ToString() + "=" + animator.GetBool(p.name).ToString() + "]\n";
                    break;
                case AnimatorControllerParameterType.Float:
                    message += "[" + p.name.ToString() + "=" + animator.GetFloat(p.name).ToString() + "]\n";
                    break;
                case AnimatorControllerParameterType.Int:
                    message += "[" + p.name.ToString() + "=" + animator.GetInteger(p.name).ToString() + "]\n";
                    break;
                case AnimatorControllerParameterType.Trigger:
                    message += "[" + p.name.ToString() + "=" + animator.GetBool(p.name).ToString() + "]\n";
                    break;
            }
        }
        message += "\n}";
        
        return message;
#else
    return "";
#endif

    }
}
