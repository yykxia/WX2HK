var TANGER_OCX_bDocOpen = false;
var TANGER_OCX_filename;
var TANGER_OCX_actionURL; //For auto generate form fiields
var TANGER_OCX_OBJ; //The Control
var TANGER_OCX_Username="Anonymous";

//Open Document From URL
function TANGER_OCX_OpenDoc(URL,USERID)
{   
    TANGER_OCX_OBJ = document.all.item("TANGER_OCX");
	var ft_num,filespec,ft_char;
	if( (typeof(URL) != "undefined") && (URL != "") )
	{	
	 ft_num=URL.substr(URL.lastIndexOf(".")+1,1);
	 if(ft_num=="2")
	    ft_char=".xls";
	 else
	    ft_char=".doc";
	 filespec=URL.substr(0,URL.length -2)+ft_char
	 var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
     xmlhttp.open("GET",filespec,false)
     xmlhttp.send()
	 if(404 == xmlhttp.status)
        newdoc(ft_char,ft_num,USERID)
	 else
	   try{TANGER_OCX_OBJ.OpenFromURL(filespec);}catch(err){};

	}
	else
	{      
try{TANGER_OCX_OBJ.OpenFromURL("default.doc");}catch(err){};
	}
	
}
function newdoc(type,sel,USERID)
{   
    if (type=='.doc')
	{TANGER_OCX_OBJ.CreateNew("Word.Document");
     TANGER_OCX_OBJ.ActiveDocument.TrackRevisions = true;}
    if (type=='.xls')
	TANGER_OCX_OBJ.CreateNew("Excel.Sheet");
	if (sel=='3')
	{TANGER_OCX_OBJ.CreateNew("Word.Document");
	 AddPictureFromLocal();}
	TANGER_OCX_OBJ.ActiveDocument.Application.UserName = USERID;
}
function TANGER_OCX_EditEnable(bz)
{
TANGER_OCX_OBJ = document.all.item("TANGER_OCX");
if(bz=="read")TANGER_OCX_OBJ.Toolbars=false;
}


function TANGER_OCX_SaveEditToServerDisk()
{
	TANGER_OCX_filename = document.all.item("filename").value+'.doc';
	if ( (typeof(TANGER_OCX_filename) == "undefined")||(!TANGER_OCX_filename) || (strtrim(TANGER_OCX_filename)==""))
	{
alert("必须输入一个文件名!");
return;
	}
	var newwin,newdoc;
	try
	{
	 	if(!TANGER_OCX_doFormOnSubmit())return; //we may do onsubmit first
	 	//call SaveToURL WITOUT other form data
var retHTML = TANGER_OCX_OBJ.SaveToURL
(
	"uploadedit.aspx",  
	"EDITFILE",	
	"", //other params seperrated by '&'. For example:myname=tanger&hisname=tom
	TANGER_OCX_filename //filename
); //this function returns dta from server
//open a new window to show the returned data
newwin = window.open("","_blank","left=200,top=200,width=400,height=300,status=0,toolbar=0,menubar=0,location=0,scrollbars=1,resizable=1",false);
newdoc = newwin.document;
newdoc.open();
newdoc.write("<html><head><title>Data returned from server:</title></head><body><center><hr>")
newdoc.write(retHTML+"<hr>");
newdoc.write("<input type=button VALUE='Close Window' onclick='window.close()'>");
newdoc.write('</center></body></html>');
newdoc.close();
if(window.opener) 
{
	window.opener.location.reload();
}
//window.close();
	}
	catch(err){
alert("err:" + err.number + ":" + err.description);
	}
	finally{
	}
}
//从本地增加印章文档指定位置
function AddSignFromLocal()
{

   if(TANGER_OCX_bDocOpen)
   {
      TANGER_OCX_OBJ.AddSignFromLocal(
	TANGER_OCX_Username,//当前登陆用户
	"",//缺省文件
	true,//提示选择
	0,//left
	0,"",1,100,0)  //top
   }
}

//从URL增加印章文档指定位置
function AddSignFromURL(URL)
{
    TANGER_OCX_OBJ.AddSignFromURL(
	TANGER_OCX_Username,//当前登陆用户
	URL,//URL
	-50,//left
	-50,"",1,100,0)  //top
}

//开始手写签名
function DoHandSign()
{
	TANGER_OCX_OBJ.DoHandSign2(TANGER_OCX_Username,"",0,0,0); //当前登陆用户 必须 //可选参数
}
//检查签名结果
function DoCheckSign()
{
	if(TANGER_OCX_bDocOpen)
	{
	var ret = TANGER_OCX_OBJ.DoCheckSign
	(
	/*可选参数 IsSilent 缺省为FAlSE，表示弹出验证对话框,否则，只是返回验证结果到返回值*/
	);//返回值，验证结果字符串
	//alert(ret);
	}	
}
function AddPictureFromLocal()
{
	if(TANGER_OCX_bDocOpen)
	{	
    TANGER_OCX_OBJ.AddPicFromLocal(
	"", //path 
	true,//prompt to select
	true,//is float
	0,//left
	0); //top
	};	
}

function AddPictureFromURL(URL)
{
	if(TANGER_OCX_bDocOpen)
	{
    TANGER_OCX_OBJ.AddPicFromURL(
	URL,//URL Note: URL must return Word supported picture types
	true,//is float
	0,//left
	0);//top
	};
}
function InsertDocFromURL(URL)
{
	if(TANGER_OCX_bDocOpen)
	{
TANGER_OCX_OBJ.AddTemplateFromURL(URL);
	};
}


function DoHandDraw()
{
	if(TANGER_OCX_bDocOpen)
	{	
	TANGER_OCX_OBJ.DoHandDraw2(
	0,0,0);//top optional
	}
}

function TANGER_OCX_AddDocHeader( strHeader )
{
	var i,cNum = 30;
	var lineStr = "";
	try
	{
for(i=0;i<cNum;i++) lineStr += "_"; 
with(TANGER_OCX_OBJ.ActiveDocument.Application)
{
	Selection.HomeKey(6,0); // go home
	Selection.TypeText(strHeader);
	Selection.TypeParagraph(); 
	Selection.TypeText(lineStr); 
	Selection.TypeText("★");
	Selection.TypeText(lineStr);  
	Selection.TypeParagraph();
	Selection.HomeKey(6,1); 
	Selection.ParagraphFormat.Alignment = 1; 
	with(Selection.Font)
	{
Name = "Arial";
Size = 12;
Bold = false;
Italic = false;
Underline = 0;
UnderlineColor = 0;
StrikeThrough = false;
DoubleStrikeThrough = false;
Outline = false;
Emboss = false;
Shadow = false;
Hidden = false;
SmallCaps = false;
AllCaps = false;
Color = 255;
Engrave = false;
Superscript = false;
Subscript = false;
Spacing = 0;
Scaling = 100;
Position = 0;
Kerning = 0;
Animation = 0;
DisableCharacterSpaceGrid = false;
EmphasisMark = 0;
	}
	Selection.MoveDown(5, 3, 0); 
}
	}
	catch(err){
//alert("err:" + err.number + ":" + err.description);
	}
	finally{
	}
}
function strtrim(value)
{
	return value.replace(/^\s+/,'').replace(/\s+$/,'');
}

function TANGER_OCX_doFormOnSubmit()
{
	var form = document.forms[0];
  	if (form.onsubmit)
	{
    	var retVal = form.onsubmit();
     	if (typeof retVal == "boolean" && retVal == false)
       	return false;
	}
	return true;
}

function TANGER_OCX_EnableReviewBar(boolvalue)
{
	TANGER_OCX_OBJ.ActiveDocument.CommandBars("Reviewing").Enabled = boolvalue;
	TANGER_OCX_OBJ.ActiveDocument.CommandBars("Track Changes").Enabled = boolvalue;
	TANGER_OCX_OBJ.IsShowToolMenu = boolvalue;
}

function TANGER_OCX_SetReviewMode(boolvalue)
{
	TANGER_OCX_OBJ.ActiveDocument.TrackRevisions = boolvalue;
}

function TANGER_OCX_SetMarkModify(boolvalue)
{
	TANGER_OCX_SetReviewMode(boolvalue);
	TANGER_OCX_EnableReviewBar(!boolvalue);
}

function TANGER_OCX_ShowRevisions(boolvalue)
{
	TANGER_OCX_OBJ.ActiveDocument.ShowRevisions = boolvalue;
}

function TANGER_OCX_PrintRevisions(boolvalue)
{
	TANGER_OCX_OBJ.ActiveDocument.PrintRevisions = boolvalue;
}

function TANGER_OCX_SetDocUser(cuser)
{
	with(TANGER_OCX_OBJ.ActiveDocument.Application)
	{
UserName = cuser;
TANGER_OCX_Username = cuser;
	}	
}

function TANGER_OCX_ChgLayout()
{
 	try
	{
TANGER_OCX_OBJ.showdialog(5); 
	}
	catch(err){
alert("err:" + err.number + ":" + err.description);
	}
	finally{
	}
}

function TANGER_OCX_PrintDoc()
{
	try
	{
TANGER_OCX_OBJ.printout(true);
	}
	catch(err){
alert("err:"  + err.number + ":" + err.description);
	}
	finally{
	}
}


function TANGER_OCX_EnableFileNewMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(0) = boolvalue;
}

function TANGER_OCX_EnableFileOpenMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(1) = boolvalue;
}

function TANGER_OCX_EnableFileCloseMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(2) = boolvalue;
}

function TANGER_OCX_EnableFileSaveMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(3) = boolvalue;
}

function TANGER_OCX_EnableFileSaveAsMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(4) = boolvalue;
}

function TANGER_OCX_EnableFilePrintMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(5) = boolvalue;
}

function TANGER_OCX_EnableFilePrintPreviewMenu(boolvalue)
{
	TANGER_OCX_OBJ.EnableFileCommand(6) = boolvalue;
}

function TANGER_OCX_OnDocumentOpened(str, obj)
{
	TANGER_OCX_bDocOpen = true;	
	TANGER_OCX_SetDocUser(TANGER_OCX_Username);
}

function TANGER_OCX_OnDocumentClosed()
{
   TANGER_OCX_bDocOpen = false;
}
//打开新窗口
function openwinincenter(url)
{
  winName="newWin";
  var awidth=500;
  var aheight=300;
  var atop=(screen.availHeight - aheight)/3;
  var aleft=(screen.availWidth - awidth)/2;
  var param0="scrollbars=0,status=0,menubar=0,resizable=2,location=0";
  var params="top=" + atop + ",left=" + aleft + ",width=" + awidth + ",height=" + aheight + "," + param0 ; 
  win=window.open(url,winName,params);
	      win.focus();   
}



