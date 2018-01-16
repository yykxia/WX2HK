using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("PLM_TreatScan")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("P R C")]
[assembly: AssemblyProduct("PLM_TreatScan")]
[assembly: AssemblyCopyright("Copyright © P R C 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("06994c45-0b5c-42dc-a4b2-bd8cd07f5460")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
//-----------------------------------------------------------------------
//模塑车间看板
//作者：戴重阳
//-----------------------------------------------------------------------
/////////////////////////////////////////////////////////////////////////
// 版本编号：1.0.0.0
// 版本说明：完成了模板的框架开发
// 版本修订人：戴重阳
// 版本日期：2017
/////////////////////////////////////////////////////////////////////////
// 版本编号：1.0.0.1
// 版本说明：
// 1、在实例化f_login窗体时添加了从注册表中读取端口，并将端口打开
// 2、在f_main窗体中添加了有参的构造函数，将登录窗体打开端口列表赋值给f_main中的list变量
// 3、在f_main窗体load的时候为每个端口分配同一个时间，并利用多线程同步进行控制收到的数据
// 4、对个别字段命名进行了修改，遵守驼峰命名法，还有部分不确定未曾更名，下次需要修改 
// 版本修订人：许建
// 版本日期：2017/12/28
/////////////////////////////////////////////////////////////////////////
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]