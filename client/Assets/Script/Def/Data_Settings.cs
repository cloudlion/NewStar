using UnityEngine;
using System.Collections;

public class Data_Settings {



    //use to separator GDS data
    public const char DATA_SEPARATOR = '\n';
    public const char DATA_SEPARATOR2 = '\r';

    public const char DATA_HYPHEN = ':';
    public const char DATA_COMMA = ',';
    public const char REALM_PREFIX = '#';
    public const char CHAR_SLASH = '/';
    public const char CHAR_CROS = '-';
    public const string CHAR_UNDERLINE = "_";
	public const string CHAR_PLUS = "+";
	public const string CHAR_SUB = "-";
    public const string REALM_TITLE = "Realm:";

    public const string SLIDER_MAX_STR = "General.Maxed";

    public const string PERSENT_SIGN = "%";

    public const string AMPERSAND_SIGN = "&";
}


public enum MailType
{
	System = 1,
	Gift = 2,
	March = 3,
	PetMarch = 4,
	User = 5,
	Alliance = 6,
	Scout = 7,
	PetScout = 8,
	Reinfroce = 9,
	Transport = 10,
    Gather = 11,
	Fail = 12,
	INVITE = 16,
	WELCOME = 17,
	UserTournament = 19,
	AllianceTournament = 20,
	AllianceReward = 21,
	TransferLeader = 22,

	SceneReward = 24,
	SystemReply = 25,
    AllianceAuctionStart = 26,
    AllianceAuctionEnd = 27,
    AllianceAuctionRefund = 28,
	MonthlyCard = 29
}

