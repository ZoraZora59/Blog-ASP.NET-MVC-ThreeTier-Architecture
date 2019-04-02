<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>URL过滤</title>
<link href="../css/terminal.css" rel="stylesheet" type="text/css" />

</head>

<script language="JavaScript" type="text/JavaScript">

// Added by csz in [2011-9-13] for the customize for malaysia: access denied page should show ip addr information etc.
var strShow = "According to the access control policy, you are not allowed to access this website. If you have any doubt, please contact the network administrator.";

function getParamString(name)
{
    var strHref = window.location.href;
	var intPos = strHref.indexOf("?");
    if(-1 == intPos)
    {
        return '';
    }
    
	var strRight = strHref.substr(intPos + 1);  // get the parameter from url
	var parameters = strRight.split("&"); 
	
    var pos, paraName, paraValue;
	
	for(var i=0; i<parameters.length; i++)
    {
        // get psoition of equality operator
        pos = parameters[i].indexOf('=');
        if(pos == -1)
        {
            continue;
        }
 
        // get name and value from url
        paraName = parameters[i].substring(0, pos);
        paraValue = parameters[i].substring(pos + 1);
 
        // if name search hit, then return current value. And restore + operator to blank space  
        if(paraName == name)
        {
            return decodeURIComponent(paraValue.replace(/\+/g, " "));
        }
    }
    
    return '';
}

function onInit()
{
    //var strIp   = getParamString("ip");
    var strType = getParamString("url_type");
    var strPlc  = getParamString("plc_name");
    
    if(strPlc.indexOf("#")>-1){
    	strPlc = strPlc.match(/(.*)#/)[1];
    }
    document.getElementById("strType").innerHTML = strType;
    document.getElementById("strPlc").innerHTML = strPlc;
}

</script>

<body onload="onInit()">
<div id="content">
  <h1 class="warning">访问被拒绝</h1>
  <div class="partition"> <span class="partition_left" style="width:220px;"></span> </div>
  <div name="ContentShow" id="ContentShow">
  	<p class='b_distance'>您尝试访问的网站类型属于[<font color=red id="strType"></font>]已经被上网策略[<font color=red id="strPlc"></font>]拒绝访问。如果有疑问，请联系网络管理员。</p>
</div>
</div>
</body>
</html>