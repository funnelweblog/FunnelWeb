/*
Name: jsDate
Desc: VBScript native Date functions emulated for Javascript
Author: Rob Eberhardt, Slingshot Solutions - http://slingfive.com/
Note: see jsDate.txt for more info
*/

// constants
vbGeneralDate = 0; vbLongDate = 1; vbShortDate = 2; vbLongTime = 3; vbShortTime = 4;  // NamedFormat
vbUseSystemDayOfWeek = 0; vbSunday = 1; vbMonday = 2; vbTuesday = 3; vbWednesday = 4; vbThursday = 5; vbFriday = 6; vbSaturday = 7; // FirstDayOfWeek
vbUseSystem = 0; vbFirstJan1 = 1; vbFirstFourDays = 2; vbFirstFullWeek = 3; // FirstWeekOfYear

// arrays (1-based)
Date.MonthNames = [null, 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
Date.WeekdayNames = [null, 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];




Date.IsDate = function(p_Expression) {
    return !isNaN(new Date(p_Expression)); 	// <-- review further
}

Date.CDate = function(p_Date) {
    if (Date.IsDate(p_Date)) { return new Date(p_Date); }

    var strTry = p_Date.toString().replace(/\-/g, '/').replace(/\./g, '/').replace(/ /g, '/'); 
    strTry = strTry.replace(/pm$/i, " pm").replace(/am$/i, " am"); // and meridian spacing
    if (Date.IsDate(strTry)) { return new Date(strTry); }

    var strTryYear = strTry + '/' + new Date().getFullYear(); // append year
    if (Date.IsDate(strTryYear)) { return new Date(strTryYear); }


    if (strTry.indexOf(":")) {	// if appears to have time
        var strTryYear2 = strTry.replace(/ /, '/' + new Date().getFullYear() + ' '); // insert year
        if (Date.IsDate(strTryYear2)) { return new Date(strTryYear2); }

        var strTryDate = new Date().toDateString() + ' ' + p_Date; // pre-pend current date
        if (Date.IsDate(strTryDate)) { return new Date(strTryDate); }
    }

    return false; // double as looser IsDate
    //throw("Error #13 - Type mismatch");	// or is this better? 
}



Date.DateAdd = function(p_Interval, p_Number, p_Date) {
    if (!Date.CDate(p_Date)) { return "invalid date: '" + p_Date + "'"; }
    if (isNaN(p_Number)) { return "invalid number: '" + p_Number + "'"; }

    p_Number = new Number(p_Number);
    var dt = Date.CDate(p_Date);

    switch (p_Interval.toLowerCase()) {
        case "yyyy": 
            {
                dt.setFullYear(dt.getFullYear() + p_Number);
                break;
            }
        case "q": 
            {
                dt.setMonth(dt.getMonth() + (p_Number * 3));
                break;
            }
        case "m": 
            {
                dt.setMonth(dt.getMonth() + p_Number);
                break;
            }
        case "y": 		// day of year
        case "d": 		// day
        case "w": 
            {		// weekday
                dt.setDate(dt.getDate() + p_Number);
                break;
            }
        case "ww": 
            {	// week of year
                dt.setDate(dt.getDate() + (p_Number * 7));
                break;
            }
        case "h": 
            {
                dt.setHours(dt.getHours() + p_Number);
                break;
            }
        case "n": 
            {		// minute
                dt.setMinutes(dt.getMinutes() + p_Number);
                break;
            }
        case "s": 
            {
                dt.setSeconds(dt.getSeconds() + p_Number);
                break;
            }
        case "ms": 
            {	// JS extension
                dt.setMilliseconds(dt.getMilliseconds() + p_Number);
                break;
            }
        default: 
            {
                return "invalid interval: '" + p_Interval + "'";
            }
    }
    return dt;
}



Date.DateDiff = function(p_Interval, p_Date1, p_Date2, p_FirstDayOfWeek) {
    if (!Date.CDate(p_Date1)) { return "invalid date: '" + p_Date1 + "'"; }
    if (!Date.CDate(p_Date2)) { return "invalid date: '" + p_Date2 + "'"; }
    p_FirstDayOfWeek = (isNaN(p_FirstDayOfWeek) || p_FirstDayOfWeek == 0) ? vbSunday : parseInt(p_FirstDayOfWeek); // set default & cast

    var dt1 = Date.CDate(p_Date1);
    var dt2 = Date.CDate(p_Date2);

    // correct DST-affected intervals ("d" & bigger)
    if ("h,n,s,ms".indexOf(p_Interval.toLowerCase()) == -1) {
        if (p_Date1.toString().indexOf(":") == -1) { dt1.setUTCHours(0, 0, 0, 0) }; // no time, assume 12am
        if (p_Date2.toString().indexOf(":") == -1) { dt2.setUTCHours(0, 0, 0, 0) }; // no time, assume 12am
    }


    // get ms between UTC dates and make into "difference" date
    var iDiffMS = dt2.valueOf() - dt1.valueOf();
    var dtDiff = new Date(iDiffMS);

    // calc various diffs
    var nYears = dt2.getUTCFullYear() - dt1.getUTCFullYear();
    var nMonths = dt2.getUTCMonth() - dt1.getUTCMonth() + (nYears != 0 ? nYears * 12 : 0);
    var nQuarters = parseInt(nMonths / 3); //<<-- different than VBScript, which watches rollover not completion

    var nMilliseconds = iDiffMS;
    var nSeconds = parseInt(iDiffMS / 1000);
    var nMinutes = parseInt(nSeconds / 60);
    var nHours = parseInt(nMinutes / 60);
    var nDays = parseInt(nHours / 24); // <-- now fixed for DST switch days
    var nWeeks = parseInt(nDays / 7);


    if (p_Interval.toLowerCase() == 'ww') {
        // set dates to 1st & last FirstDayOfWeek
        var offset = Date.DatePart("w", dt1, p_FirstDayOfWeek) - 1;
        if (offset) { dt1.setDate(dt1.getDate() + 7 - offset); }
        var offset = Date.DatePart("w", dt2, p_FirstDayOfWeek) - 1;
        if (offset) { dt2.setDate(dt2.getDate() - offset); }
        // recurse to "w" with adjusted dates
        var nCalWeeks = Date.DateDiff("w", dt1, dt2) + 1;
    }
    // TODO: similar for 'w'?


    // return difference
    switch (p_Interval.toLowerCase()) {
        case "yyyy": return nYears;
        case "q": return nQuarters;
        case "m": return nMonths;
        case "y": 		// day of year
        case "d": return nDays;
        case "w": return nWeeks;
        case "ww": return nCalWeeks; // week of year	
        case "h": return nHours;
        case "n": return nMinutes;
        case "s": return nSeconds;
        case "ms": return nMilliseconds; // not in VBScript
        default: return "invalid interval: '" + p_Interval + "'";
    }
}




Date.DatePart = function(p_Interval, p_Date, p_FirstDayOfWeek) {
    if (!Date.CDate(p_Date)) { return "invalid date: '" + p_Date + "'"; }

    var dtPart = Date.CDate(p_Date);

    switch (p_Interval.toLowerCase()) {
        case "yyyy": return dtPart.getFullYear();
        case "q": return parseInt(dtPart.getMonth() / 3) + 1;
        case "m": return dtPart.getMonth() + 1;
        case "y": return Date.DateDiff("y", "1/1/" + dtPart.getFullYear(), dtPart) + 1; // day of year
        case "d": return dtPart.getDate();
        case "w": return Date.Weekday(dtPart.getDay() + 1, p_FirstDayOfWeek); 	// weekday
        case "ww": return Date.DateDiff("ww", "1/1/" + dtPart.getFullYear(), dtPart, p_FirstDayOfWeek) + 1; // week of year
        case "h": return dtPart.getHours();
        case "n": return dtPart.getMinutes();
        case "s": return dtPart.getSeconds();
        case "ms": return dtPart.getMilliseconds(); // <-- JS extension, NOT in VBScript
        default: return "invalid interval: '" + p_Interval + "'";
    }
}



Date.MonthName = function(p_Month, p_Abbreviate) {
    if (isNaN(p_Month)) {	// v0.94- compat: extract real param from passed date
        if (!Date.CDate(p_Month)) { return "invalid month: '" + p_Month + "'"; }
        p_Month = DatePart("m", Date.CDate(p_Month));
    }

    var retVal = Date.MonthNames[p_Month];
    if (p_Abbreviate == true) { retVal = retVal.substring(0, 3) } // abbr to 3 chars
    return retVal;
}


Date.WeekdayName = function(p_Weekday, p_Abbreviate, p_FirstDayOfWeek) {
    if (isNaN(p_Weekday)) {	// v0.94- compat: extract real param from passed date
        if (!Date.CDate(p_Weekday)) { return "invalid weekday: '" + p_Weekday + "'"; }
        p_Weekday = DatePart("w", Date.CDate(p_Weekday));
    }
    p_FirstDayOfWeek = (isNaN(p_FirstDayOfWeek) || p_FirstDayOfWeek == 0) ? vbSunday : parseInt(p_FirstDayOfWeek); // set default & cast

    var nWeekdayNameIdx = ((p_FirstDayOfWeek - 1 + parseInt(p_Weekday) - 1 + 7) % 7) + 1; // compensate nWeekdayNameIdx for p_FirstDayOfWeek
    var retVal = Date.WeekdayNames[nWeekdayNameIdx];
    if (p_Abbreviate == true) { retVal = retVal.substring(0, 3) } // abbr to 3 chars
    return retVal;
}


// adjusts weekday for week starting on p_FirstDayOfWeek
Date.Weekday = function(p_Weekday, p_FirstDayOfWeek) {
    p_FirstDayOfWeek = (isNaN(p_FirstDayOfWeek) || p_FirstDayOfWeek == 0) ? vbSunday : parseInt(p_FirstDayOfWeek); // set default & cast

    return ((parseInt(p_Weekday) - p_FirstDayOfWeek + 7) % 7) + 1;
}





Date.FormatDateTime = function(p_Date, p_NamedFormat) {
    if (p_Date.toUpperCase().substring(0, 3) == "NOW") { p_Date = new Date() };
    if (!Date.CDate(p_Date)) { return "invalid date: '" + p_Date + "'"; }
    if (isNaN(p_NamedFormat)) { p_NamedFormat = vbGeneralDate };

    var dt = Date.CDate(p_Date);

    switch (parseInt(p_NamedFormat)) {
        case vbGeneralDate: return dt.toString();
        case vbLongDate: return Format(p_Date, 'DDDD, MMMM D, YYYY');
        case vbShortDate: return Format(p_Date, 'MM/DD/YYYY');
        case vbLongTime: return dt.toLocaleTimeString();
        case vbShortTime: return Format(p_Date, 'HH:MM:SS');
        default: return "invalid NamedFormat: '" + p_NamedFormat + "'";
    }
}


Date.Format = function(p_Date, p_Format, p_FirstDayOfWeek, p_firstweekofyear) {
    if (!Date.CDate(p_Date)) { return "invalid date: '" + p_Date + "'"; }
    if (!p_Format || p_Format == '') { return dt.toString() };

    var dt = Date.CDate(p_Date);

    // Zero-padding formatter
    this.pad = function(p_str) {
        if (p_str.toString().length == 1) { p_str = '0' + p_str }
        return p_str;
    }

    var ampm = dt.getHours() >= 12 ? 'PM' : 'AM'
    var hr = dt.getHours();
    if (hr == 0) { hr = 12 };
    if (hr > 12) { hr -= 12 };
    var strShortTime = hr + ':' + this.pad(dt.getMinutes()) + ':' + this.pad(dt.getSeconds()) + ' ' + ampm;
    var strShortDate = (dt.getMonth() + 1) + '/' + dt.getDate() + '/' + new String(dt.getFullYear()).substring(2, 4);
    var strLongDate = Date.MonthName(dt.getMonth() + 1) + ' ' + dt.getDate() + ', ' + dt.getFullYear(); 	//

    var retVal = p_Format;

    // switch tokens whose alpha replacements could be accidentally captured
    retVal = retVal.replace(new RegExp('C', 'gi'), 'CCCC');
    retVal = retVal.replace(new RegExp('mmmm', 'gi'), 'XXXX');
    retVal = retVal.replace(new RegExp('mmm', 'gi'), 'XXX');
    retVal = retVal.replace(new RegExp('dddddd', 'gi'), 'AAAAAA');
    retVal = retVal.replace(new RegExp('ddddd', 'gi'), 'AAAAA');
    retVal = retVal.replace(new RegExp('dddd', 'gi'), 'AAAA');
    retVal = retVal.replace(new RegExp('ddd', 'gi'), 'AAA');
    retVal = retVal.replace(new RegExp('timezone', 'gi'), 'ZZZZ');
    retVal = retVal.replace(new RegExp('time24', 'gi'), 'TTTT');
    retVal = retVal.replace(new RegExp('time', 'gi'), 'TTT');

    // now do simple token replacements
    retVal = retVal.replace(new RegExp('yyyy', 'gi'), dt.getFullYear());
    retVal = retVal.replace(new RegExp('yy', 'gi'), new String(dt.getFullYear()).substring(2, 4));
    retVal = retVal.replace(new RegExp('y', 'gi'), Date.DatePart("y", dt));
    retVal = retVal.replace(new RegExp('q', 'gi'), Date.DatePart("q", dt));
    retVal = retVal.replace(new RegExp('mm', 'gi'), (dt.getMonth() + 1));
    retVal = retVal.replace(new RegExp('m', 'gi'), (dt.getMonth() + 1));
    retVal = retVal.replace(new RegExp('dd', 'gi'), this.pad(dt.getDate()));
    retVal = retVal.replace(new RegExp('d', 'gi'), dt.getDate());
    retVal = retVal.replace(new RegExp('hh', 'gi'), this.pad(dt.getHours()));
    retVal = retVal.replace(new RegExp('h', 'gi'), dt.getHours());
    retVal = retVal.replace(new RegExp('nn', 'gi'), this.pad(dt.getMinutes()));
    retVal = retVal.replace(new RegExp('n', 'gi'), dt.getMinutes());
    retVal = retVal.replace(new RegExp('ss', 'gi'), this.pad(dt.getSeconds()));
    retVal = retVal.replace(new RegExp('s', 'gi'), dt.getSeconds());
    retVal = retVal.replace(new RegExp('t t t t t', 'gi'), strShortTime);
    retVal = retVal.replace(new RegExp('am/pm', 'g'), dt.getHours() >= 12 ? 'pm' : 'am');
    retVal = retVal.replace(new RegExp('AM/PM', 'g'), dt.getHours() >= 12 ? 'PM' : 'AM');
    retVal = retVal.replace(new RegExp('a/p', 'g'), dt.getHours() >= 12 ? 'p' : 'a');
    retVal = retVal.replace(new RegExp('A/P', 'g'), dt.getHours() >= 12 ? 'P' : 'A');
    retVal = retVal.replace(new RegExp('AMPM', 'g'), dt.getHours() >= 12 ? 'pm' : 'am');
    // (always proceed largest same-lettered token to smallest)

    // now finish the previously set-aside tokens 
    retVal = retVal.replace(new RegExp('XXXX', 'gi'), Date.MonthName(dt.getMonth() + 1, false)); //
    retVal = retVal.replace(new RegExp('XXX', 'gi'), Date.MonthName(dt.getMonth() + 1, true)); //
    retVal = retVal.replace(new RegExp('AAAAAA', 'gi'), strLongDate);
    retVal = retVal.replace(new RegExp('AAAAA', 'gi'), strShortDate);
    retVal = retVal.replace(new RegExp('AAAA', 'gi'), Date.WeekdayName(dt.getDay() + 1, false, p_FirstDayOfWeek)); // 
    retVal = retVal.replace(new RegExp('AAA', 'gi'), Date.WeekdayName(dt.getDay() + 1, true, p_FirstDayOfWeek)); // 
    retVal = retVal.replace(new RegExp('TTTT', 'gi'), dt.getHours() + ':' + this.pad(dt.getMinutes()));
    retVal = retVal.replace(new RegExp('TTT', 'gi'), hr + ':' + this.pad(dt.getMinutes()) + ' ' + ampm);
    retVal = retVal.replace(new RegExp('CCCC', 'gi'), strShortDate + ' ' + strShortTime);

    // finally timezone
    tz = dt.getTimezoneOffset();
    timezone = (tz < 0) ? ('GMT-' + tz / 60) : (tz == 0) ? ('GMT') : ('GMT+' + tz / 60);
    retVal = retVal.replace(new RegExp('ZZZZ', 'gi'), timezone);

    return retVal;
}



// ====================================

/* if desired, map new methods to direct functions
*/
IsDate = Date.IsDate;
CDate = Date.CDate;
DateAdd = Date.DateAdd;
DateDiff = Date.DateDiff;
DatePart = Date.DatePart;
MonthName = Date.MonthName;
WeekdayName = Date.WeekdayName;
Weekday = Date.Weekday;
FormatDateTime = Date.FormatDateTime;
Format = Date.Format;



/* and other capitalizations for easier porting
isDate = IsDate;
dateAdd = DateAdd;
dateDiff = DateDiff;
datePart = DatePart;
monthName = MonthName;
weekdayName = WeekdayName;
formatDateTime = FormatDateTime;
format = Format;

isdate = IsDate;
dateadd = DateAdd;
datediff = DateDiff;
datepart = DatePart;
monthname = MonthName;
weekdayname = WeekdayName;
formatdatetime = FormatDateTime;

ISDATE = IsDate;
DATEADD = DateAdd;
DATEDIFF = DateDiff;
DATEPART = DatePart;
MONTHNAME = MonthName;
WEEKDAYNAME = WeekdayName;
FORMATDATETIME = FormatDateTime;
FORMAT = Format;
*/

/*
* JavaScript Pretty Date
* Copyright (c) 2008 John Resig (jquery.com)
* Licensed under the MIT license.
*/

// Takes an ISO time and returns a string representing how
// long ago the date represents.
function prettyDate(date) {
	var diff = (((new Date()).getTime() - date.getTime()) / 1000);
    var day_diff = Math.floor(diff / 86400);

    if (isNaN(day_diff) || day_diff < 0 || day_diff >= 31)
        return;

    return day_diff == 0 && (
			diff < 60 && "Just now" ||
			diff < 120 && "1 minute ago" ||
			diff < 3600 && Math.floor(diff / 60) + " minutes ago" ||
			diff < 7200 && "1 hour ago" ||
			diff < 86400 && Math.floor(diff / 3600) + " hours ago") ||
		day_diff == 1 && "Yesterday" ||
		day_diff < 7 && day_diff + " days ago" ||
		day_diff < 31 && Math.ceil(day_diff / 7) + " weeks ago";
}

