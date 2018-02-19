using UnityEngine;
using System.Collections;

namespace GameUtil
{
	public static class Util_Convert
	{
		const string DIGITS = "sFtiJqgIycvbfxeQP5jowRuM4nTrO7akd9813GAlVUHS6XDzWL2hB0KECZNmYp";//62个字符

		public static string ID2String(this long id, int radix = 62)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			char[] map = DIGITS.ToCharArray ();
			while(id > 0)
			{
				long index = id%radix;
				sb.Insert(0, map[index] );
				id /= radix;
			}

			return sb.ToString ();
		}

		public static long String2ID(this string str, int radix = 62)
		{
			long id = 0;
            if (str.IndexOf(Data_Settings.REALM_PREFIX) == 0)
            {
                str = str.Substring(1, str.Length - 1);
            }
			char[] cs = str.ToCharArray();
			for(int i = 0; i< str.Length; i++)
			{
				long t = POW(DIGITS.Length,i);
				long ind = DIGITS.IndexOf (cs [str.Length - i - 1]);
				id += long.Parse ((ind * t).ToString ());
			}
			return id;
		}


		public static long POW(int bas,int i)
		{
			long result = 0;
			if (i == 0) {
				result = 1;
			} else {
				result = bas;
				for (int j = 1; j < i; j++) {
					result = result * bas;
				}
			}
			return result;
		}

	//	const SEP = 'j';//j为分割符
	//	const BASE = '61';
	//	
	//	/**
	//     * 进制转换
	//     * @param string $input
	//     * @param string $to_base
	//     * @return string
	//     */
	//	public static string dec2any(stirng input, string to_base)
	//	{
	//		$digits = self::DIGITS;
	//		$result = '';
	//		$quotient = $input;
	//		$residue = 0;
	//		while ($quotient != '0') {
	//			$residue = bcmod($quotient, $to_base);
	//			$result = $digits{intval($residue)} . $result;
	//			$quotient = bcdiv($quotient, $to_base);
	//		}
	//		return $result;
	//	}
	//	
	//	/**
	//     * 进制转换
	//     * @param string $input
	//     * @param string $base
	//     * @return string
	//     */
	//	public static function any2dec($input, $base)
	//	{
	//		$digits = self::DIGITS;
	//		$len = strlen($input);
	//		$result = 0;
	//		for ($i = 0; $i < $len; $i++) {
	//			$digit = strpos($digits, $input{$len - $i - 1});
	//			$result = bcadd($result, bcmul($digit, bcpow($base, $i)));
	//		}
	//		return $result;
	//	}
	//	
	//	public static function encode($arr, $base)
	//	{
	//		$input = array();
	//		foreach ($arr as $id) {
	//			$input[] = self::dec2any(strval($id), $base);
	//		}
	//		return implode(self::SEP, $input);
	//	}
	//	
	//	public static function decode($ref, $base)
	//	{
	//		$arr = explode(self::SEP, $ref);
	//		$ret = array();
	//		foreach($arr as $item){
	//			$ret[] = self::any2dec($item, $base);
	//		}
	//		return $ret;
	//	}
	//	/**
	//     * 数组编码成61进制数字
	//     * @param type $array
	//     * @return type
	//     */
	//	public static function toLong(array $array)
	//	{
	//		$ref = self::encode($array, self::BASE);
	//		$long = self::any2dec($ref, self::BASE);
	//		return $long;
	//	}
	//	
	//	/**
	//     * 把61进制长数字解码成数组
	//     * @param string $long
	//     * @return array
	//     */
	//	static function toArray($long)
	//	{
	//		$ref = self::dec2any(strval($long), self::BASE);
	//		$arr = self::decode(strval($ref), self::BASE);
	//		return $arr;
	//	}
		
	}
}