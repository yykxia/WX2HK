using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using IETCsoft.sql;
using System.Data;

namespace WX2HK
{
    /// <summary>
    /// PLM_WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PLM_WebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 是否web服务处于可连接状态
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool isCnnected() 
        {
            return true;
        }

        /// <summary>
        /// 系统更新信息
        /// </summary>
        /// <param name="updateInfo">更新内容</param>
        /// <param name="sysName">检查更新系统名称</param>
        /// <param name="custVer">客户端版本</param>
        /// <returns></returns>
        [WebMethod]
        public bool existNewVersion(ref DataSet updateInfo, string sysName,string custVer) 
        {
            string sqlCmd = "select * from PLM_Version where sysName='" + sysName + "' and isNew='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "updateInfo";
            if (dt.Rows.Count == 1)
            {
                updateInfo.Tables.Add(dt);
                string serverVer = dt.Rows[0]["curVers"].ToString();
                if (custVer != serverVer)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// 货架下线
        /// </summary>
        /// <param name="optTime">执行时间</param>
        /// <param name="barCode">货架条码</param>
        /// <param name="optClient">操作终端</param>
        [WebMethod]
        public int offLine(ref DateTime optTime,string barCode,string optClient) 
        {
            int result = 0;
            string sqlCmd = "select * from PLM_Serials_BindBarCode where BarCode='" + barCode + "' and OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count == 1)
            {
                string serialsId = dt.Rows[0]["id"].ToString();//上线流水id

                //更改上线状态
                sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where id='" + serialsId + "'";
                SqlSel.ExeSql(sqlCmd);

                optTime = DateTime.Now;
                //插入下线清单
                sqlCmd = "insert into PLM_Product_OffLine (onlineId,optUser,optTime) values ('" + serialsId + "','" + optClient + "','" + optTime + "')";
                SqlSel.ExeSql(sqlCmd);

                result = 1;//返回执行结果
            }
            return result;
        }

        /// <summary>
        /// 剪边流水线班次记录验证
        /// </summary>
        /// <param name="loginClass">登录班次</param>
        /// <param name="loginGroup">登录班组</param>
        /// <param name="barCode">吊挂条码</param>
        /// <param name="loginType">记录类型(1:上工;2:收工)</param>
        /// <returns>1:记录成功，0：重复登录，-1：吊挂条码无效,3:班次已登录或已完工</returns>
        [WebMethod]
        public int WH_Treat_flowLogin_isValid(string loginClass, string loginGroup,string barCode,string loginType) 
        {
            int result = 0;
            string sqlCmd = "";
            sqlCmd = "select * from PLM_Serials_BindBarCode where BarCode='" + barCode + "' and OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count == 1)
            {
                int serialsId = Convert.ToInt32(dt.Rows[0]["id"]);//上线流水id
                string loginDate = string.Empty;//班次日期
                if (loginClass == "D" & loginType == "2")//夜班收工记录为前一天的上工记录
                {
                    loginDate = DateTime.Now.AddDays(-1).ToShortDateString();
                }
                else
                {
                    loginDate = DateTime.Now.ToShortDateString();
                }
                sqlCmd = "select * from PLM_WH_TreatRecord_FlowLine where RecordDate='" + loginDate + "' and loginClass='" + loginClass + "'";
                sqlCmd += " and RecordType='" + loginType + "' and loginGroup='" + loginGroup + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count == 0) 
                {
                    //获取当前班组最后一条记录类型
                    sqlCmd = "select RecordType,RecordId from PLM_WH_TreatRecord_FlowLine where id=(select max(id) from PLM_WH_TreatRecord_FlowLine where loginGroup='" + loginGroup + "')";
                    DataTable tempDt = new DataTable();
                    SqlSel.GetSqlSel(ref tempDt, sqlCmd);
                    if (tempDt.Rows.Count > 0)
                    {
                        string lastRecordType = tempDt.Rows[0]["RecordType"].ToString();
                        if (loginType == lastRecordType) //如果记录类型相同则不可登录
                        {
                            result = 3;
                        }
                        else
                        {
                            //验证该吊挂是否已剪边
                            int lastSerialsId = Convert.ToInt32(tempDt.Rows[0]["RecordId"]);
                            if (serialsId <= lastSerialsId)
                            {
                                result = -1;
                            }
                            else
                            {
                                sqlCmd = "insert into PLM_WH_TreatRecord_FlowLine (loginClass,loginGroup,RecordTime,RecordId,RecordType,RecordDate) values";
                                sqlCmd += " ('" + loginClass + "','" + loginGroup + "','" + DateTime.Now + "','" + serialsId + "','" + loginType + "','" + loginDate + "')";
                                SqlSel.ExeSql(sqlCmd);
                                return 1;
                            }
                        }
                    }
                    else 
                    {
                        sqlCmd = "insert into PLM_WH_TreatRecord_FlowLine (loginClass,loginGroup,RecordTime,RecordId,RecordType,RecordDate) values";
                        sqlCmd += " ('" + loginClass + "','" + loginGroup + "','" + DateTime.Now + "','" + serialsId + "','" + loginType + "','" + loginDate + "')";
                        SqlSel.ExeSql(sqlCmd);
                        return 1;
                    }
                    //if (loginType == "2") 
                    //{
                    //    //是否有班次日期的上工记录
                    //    sqlCmd = "slect * from PLM_WH_TreatRecord_FlowLine where RecordDate='" + loginDate + "' and loginClass='" + loginClass + "' and RecordType='1'";
                    //    DataTable tempDt = new DataTable();
                    //    SqlSel.GetSqlSel(ref tempDt, sqlCmd);
                    //    if (tempDt.Rows.Count == 0)
                    //    {
                    //        result = 3;
                    //    }
                    //    else 
                    //    {
                    //        //验证该吊挂是否已剪边
                    //    }
                    //}
                }
            }
            else 
            {
                result = -1;
            }


            return result;
        }

        /// <summary>
        /// 查询当前日期该班组的上工状态
        /// </summary>
        /// <param name="loginGroup">登录班组</param>
        /// <returns>1:上工;2:下工</returns>
        [WebMethod]
        public int WH_Treat_LoginType(string loginGroup)
        {
            string sqlCmd = "select * from PLM_WH_TreatRecord_FlowLine where id=";
            sqlCmd += "(select max(id) from PLM_WH_TreatRecord_FlowLine where loginGroup='" + loginGroup + "')";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                int curLoginType = Convert.ToInt32(dt.Rows[0]["RecordType"]);
                return curLoginType;
            }
            else 
            {
                return 0;
            }
            
        }
        /// <summary>
        /// 查询区间内剪边产量
        /// </summary>
        /// <param name="TreatCount">返回的订单信息</param>
        /// <param name="CountClass">班次信息</param>
        /// <param name="CountGroup">班组信息</param>
        /// <param name="barCode">条码</param>
        /// <returns></returns>
        [WebMethod]
        public int WH_Treat_intevalCount_FlowLine(ref DataTable TreatCount,string CountClass, string CountGroup, string barCode)
        {
            int result = 0;
            string sqlCmd = "";
            TreatCount.TableName = "TreatCount";
            sqlCmd = "select * from PLM_Serials_BindBarCode where BarCode='" + barCode + "' and OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count == 1)
            {
                int serialsId = Convert.ToInt32(dt.Rows[0]["id"]);//上线流水id
                DataTable tempDt = new DataTable();
                sqlCmd = "select * from PLM_WH_TreatRecord_FlowLine where id=(select max(id) from PLM_WH_TreatRecord_FlowLine where loginGroup='" + CountGroup + "')";
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    //验证该吊挂是否已剪边
                    int lastSerialsId = Convert.ToInt32(tempDt.Rows[0]["RecordId"]);
                    if (serialsId <= lastSerialsId)
                    {
                        result = -1;
                    }
                    else
                    {                       
                        sqlCmd = "select TradeNo,SUM(BindQty) from View_PLM_LineGroup_BindSerials where id>'" + lastSerialsId + "' ";
                        sqlCmd += " and id<'" + serialsId + "' and LineGroup='" + CountGroup + "' group by TradeNo";
                        SqlSel.GetSqlSel(ref dt, sqlCmd);
                        return 1;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 查询当前条码绑定的订单信息
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_queryOrderInfoByBarCode(string barCode)
        {
            string sqlCmd = "select ('B'+ convert(nvarchar(50),A.id)) as recordId,A.*,B.orderNo,B.ItemNo,B.ItemParm from PLM_Serials_BindBarCode A left join PLM_Product_OnLine B";
            sqlCmd += " ON A.tradeNo=B.id";
            sqlCmd += " where A.BarCode='" + barCode + "' and A.OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "orderInfo";
            return dt;
        }

        ///<summary>
        ///验证是否是有效的库位条码
        ///</summary>
        ///<param name="barCode">条码号</param>
        ///<returns></returns>
        [WebMethod]
        public bool isValidStorageCode(string barCode) 
        {
            string sqlCmd = "select * from PLM_WH_Storage where StorageCode='" + barCode + "' and UseStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        ///<summary>
        ///根据sql返回相应结果集
        ///</summary>
        ///<param name="sqlStr">sql查询语句</param>
        ///<returns></returns>
        [WebMethod]
        public DataTable getData(string sqlStr) 
        {
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlStr);
            dt.TableName = "getData";
            return dt;
        }


        ///<summary>
        ///执行相关sql语句
        ///</summary>
        ///<param name="sqlStr">sql执行语句</param>
        ///<returns></returns>
        [WebMethod]
        public int ExecuteData(string sqlStr)
        {
            int result = SqlSel.ExeSql(sqlStr);
            return result;
        }

        /// <summary>
        /// 查询当前条码绑定的订单信息:篓车
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_queryOrderInfoByBarCode_T(string barCode)
        {
            string sqlCmd = "select ('T'+ convert(nvarchar(50),A.id)) as recordId,B.orderNo,transferCount,";
            sqlCmd += "B.ItemNo,B.ItemParm,A.orderId from PLM_WH_Transfer_OrderBind A left join PLM_Product_OnLine B";
            sqlCmd += " ON A.orderId=B.id Left join PLM_WH_Transfer C on C.id=A.TransferId where A.TransferId='" + barCode + "' and bindStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "OrderNo_T";
            return dt;
        }


        /// <summary>
        /// 查询当前条码绑定的订单信息:卸绵
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_queryOrderInfoByBarCode_S(string barCode)
        {
            string sqlCmd = "select ('B'+ convert(nvarchar(50),A.id)) as recordId,A.*,B.orderNo,B.ItemNo,B.ItemParm from PLM_Serials_BindBarCode A left join PLM_Product_OnLine B";
            sqlCmd += " ON A.tradeNo=B.id";
            sqlCmd += " where A.BarCode='" + barCode + "' and A.OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "OrderNo_S";
            return dt;
        }

        /// <summary>
        /// 当前容器是否已在库
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns>1:在库;0:可入库</returns>
        [WebMethod]
        public int isBoundState(string barCode) 
        {
            string sqlCmd = "select * from PLM_WH_Storage_Actual where CntrCode='" + barCode + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count >= 1)
            {
                return 1;
            }
            else 
            {
                return 0;
            }
        }

        /// <summary>
        /// 出入库操作
        /// </summary>
        /// <param name="BarCode">条码号</param>
        /// <param name="StorageCode">库位号</param>
        /// <param name="RecordId">来源id</param>
        /// <param name="BoundStatus">0:出库;1:入库</param>
        /// <param name="onLineId">排产id</param>
        /// <param name="BoundQty">数量</param>
        [WebMethod]
        public void Storage_BoundState(string BarCode, string StorageCode, string RecordId, string BoundStatus, string onLineId, string BoundQty)
        {
            string sqlCmd = "insert into PLM_WH_InBound_Record (BarCode,StorageCode,RecordId,BoundStatus,onLineId,BoundQty,boundTime)";
            sqlCmd += "values ('" + BarCode + "','" + StorageCode + "','" + RecordId + "','" + BoundStatus + "','" + onLineId + "','" + BoundQty + "','" + DateTime.Now + "')";
            SqlSel.ExeSql(sqlCmd);
            if (BoundStatus == "1")//入库操作
            {
                if (BarCode != "0000")//非直接卸绵
                {
                    //更新当前库位可用数
                    if (isBoundState(BarCode) == 0)//如果不是一对多的绑定
                    {
                        StoragePos(StorageCode, "-1");
                    }

                    sqlCmd = "insert into PLM_WH_Storage_Actual (StorageCode,OnlineId,CntrCode,BoundQty,BoundTime) values ('" + StorageCode + "',";
                    sqlCmd += "'" + onLineId + "','" + BarCode + "','" + BoundQty + "','" + DateTime.Now + "')";
                    if (SqlSel.ExeSql(sqlCmd) > 0)
                    {
                        //如果来源号不为空，则更新货架或篓车与物料的绑定状态
                        if (RecordId.StartsWith("B"))
                        {
                            //增加入库标志
                            sqlCmd = "update PLM_Serials_BindBarCode set storageSign='1' where id='" + RecordId.Substring(1) + "'";
                            SqlSel.ExeSql(sqlCmd);
                        }
                    }

                }
                else
                {
                    //该库位是否已有该订单的散放入库数量
                    sqlCmd = "select * from PLM_WH_Storage_Actual where StorageCode='" + StorageCode + "' and CntrCode='0000' and OnlineId='" + onLineId + "'";
                    DataTable orderBoundCount = new DataTable();
                    SqlSel.GetSqlSel(ref orderBoundCount, sqlCmd);
                    if (orderBoundCount.Rows.Count == 1)
                    {
                        //有，直接更新数量
                        sqlCmd = "update PLM_WH_Storage_Actual set BoundQty=BoundQty" + BoundQty + " where id='" + orderBoundCount.Rows[0]["id"].ToString() + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }
                    else
                    {
                        //没有则插入一条散放入库记录
                        sqlCmd = "insert into PLM_WH_Storage_Actual (StorageCode,OnlineId,CntrCode,BoundQty,BoundTime) values ('" + StorageCode + "',";
                        sqlCmd += "'" + onLineId + "','" + BarCode + "','" + BoundQty + "','" + DateTime.Now + "')";
                        SqlSel.ExeSql(sqlCmd);
                    }
                    //如果来源号不为空，则更新货架或篓车与物料的绑定状态
                    if (RecordId.StartsWith("B"))
                    {
                        sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where id='" + RecordId.Substring(1) + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }
                }

            }
            if (BoundStatus == "0")//出库 
            {
                sqlCmd = "select * from PLM_WH_Storage_Actual where StorageCode='" + StorageCode + "' and CntrCode='" + BarCode + "' and OnlineId='" + onLineId + "'";
                DataTable dt = new DataTable();
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                if (dt.Rows.Count == 1)
                {
                    int actualQty = Convert.ToInt32(dt.Rows[0]["BoundQty"]);//当前货位数量
                    int unBoundQty = Convert.ToInt32(BoundQty);//出库数量
                    if (actualQty == unBoundQty)
                    {
                        //更新库位绑定情况
                        sqlCmd = "delete from PLM_WH_Storage_Actual where id='" + dt.Rows[0]["id"].ToString() + "'";
                        SqlSel.ExeSql(sqlCmd);

                        if (BarCode != "0000")//非散放出库
                        {
                            if (isBoundState(BarCode) == 0)
                            {
                                //释放容器，恢复可用
                                if (BarCode.StartsWith("B"))
                                {
                                    //更新当前库位可用数
                                    StoragePos(StorageCode, "+1");
                                    //如果来源号不为空，则更新货架或篓车与物料的绑定状态
                                    if (RecordId.StartsWith("B"))
                                    {
                                        sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where BarCode='" + BarCode + "' and OlineStatus='1' and tradeNo='" + onLineId + "'";
                                        SqlSel.ExeSql(sqlCmd);
                                    }
                                }
                                if (BarCode.StartsWith("T"))
                                {
                                    //更新当前库位可用数
                                    StoragePos(StorageCode, "+1");
                                    //如果来源号不为空，则更新货架或篓车与物料的绑定状态
                                    if (RecordId.StartsWith("T"))
                                    {
                                        string trasferId = BarCode.Substring(1);
                                        sqlCmd = "update PLM_WH_Transfer set bindStatus='0' where id='" + trasferId + "'";
                                        SqlSel.ExeSql(sqlCmd);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        //更新库位订单数量
                        sqlCmd = "update PLM_WH_Storage_Actual set BoundQty=BoundQty-" + BoundQty + " where id='" + dt.Rows[0]["id"].ToString() + "'";
                        SqlSel.ExeSql(sqlCmd);
                    }


                }
            }
        }

        /// <summary>
        /// 操作仓库可用库位数
        /// </summary>
        /// <param name="storageCode">库位号</param>
        /// <param name="optType">出库:-1;入库:-1</param>
        [WebMethod]
        public void StoragePos(string storageCode, string optType) 
        {
            string sqlCmd = "update PLM_WH_Storage set AvailablePos=AvailablePos " + optType + " where StorageCode='" + storageCode + "'";
            SqlSel.ExeSql(sqlCmd);
        }

        /// <summary>
        /// 待配送的吊挂下线
        /// </summary>
        /// <param name="lineGroup">所属班组</param>
        /// <returns>返回已存在下线未全部配送至篓车的生产订单信息</returns>
        [WebMethod]
        public DataTable Transfer_offLineList(string lineGroup) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "SELECT A.TradeNo,B.SQty,(SQty-ISNULL(transferCount,0)) as remainNumb,";
            sqlCmd += "D.orderNo,D.ItemParm,D.ItemNo,D.ItemName FROM ";
            sqlCmd += "(SELECT tradeNo FROM PLM_Product_OffLine A LEFT JOIN PLM_Serials_BindBarCode B";
            sqlCmd += " ON A.onlineId=B.ID WHERE A.optUser='" + lineGroup + "' GROUP BY tradeNo) A";
            sqlCmd += " LEFT JOIN (SELECT ProdId,MIN(OrderId) AS OrderId FROM PLM_Product_Rel GROUP BY ProdId) T1";
            sqlCmd += " ON T1.ProdId=A.TradeNo LEFT JOIN View_ERP_SCDDCP T2 ON T2.SCDDCP_LSBH=T1.OrderId";
            sqlCmd += " LEFT JOIN View_PLM_ProducedCount B ON A.tradeNo=B.TradeNo";
            sqlCmd += " LEFT JOIN (select orderId,SUM(transferCount) AS transferCount from PLM_WH_Transfer_OrderBind group by orderId) C";
            sqlCmd += " ON C.orderId=A.tradeNo LEFT JOIN PLM_Product_online D ON D.id=A.tradeNo WHERE ISNULL(transferCount,0)<SQty";
            sqlCmd += " and A.tradeNo not in (select OnlineId from PLM_Product_Closed)";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            //for (int i = 0; i < dt.Rows.Count; i++) 
            //{
            //    int transferCount = Convert.ToInt32(dt.Rows[i]["transferCount"]);//已装数量
            //    int SQty = Convert.ToInt32(dt.Rows[i]["SQty"]);//吊挂总数
            //    if (transferCount >= SQty) 
            //    {

            //    }
            //}
            dt.TableName = "offLine_Order";
            return dt;
        }

        /// <summary>
        /// 根据生产订单号查询订单及入库信息
        /// </summary>
        /// <param name="orderId">生产订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable Tranfer_OffLine_queryOrder(string orderId)
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select t1.Id,t1.ItemName,t1.ItemNo,t1.OrderNo,t1.ItemParm,t1.ItemTech,t2.prodNumb,t1.PlanCount,";
            sqlCmd += " ISNULL(transferCount,0) AS transferCount,t4.SCDDCP_JHTCSL,t4.SCDDCP_JHPCH,t5.SQty from PLM_Product_OnLine t1";
            sqlCmd += " left join (select sum(bindQty) as prodNumb,TradeNo from PLM_Serials_BindBarCode group by TradeNo) t2 on t1.id=t2.TradeNo";
            sqlCmd += " LEFT JOIN (SELECT ProdId,MIN(OrderId) AS OrderId FROM PLM_Product_Rel GROUP BY ProdId) T3 ON T3.ProdId=t1.Id";
            sqlCmd += " LEFT JOIN View_PLM_ProducedCount t5 ON t5.TradeNo=t1.id";
            sqlCmd += " LEFT JOIN View_ERP_SCDDCP T4 ON T4.SCDDCP_LSBH=T3.OrderId where t1.id='" + orderId + "'";

            //string sqlCmd = "select A.TradeNo,SQty as remainNumb,";
            //sqlCmd += "C.orderNo,C.ItemParm,C.ItemNo,C.ItemName from View_PLM_ProducedCount A";
            //sqlCmd += " LEFT JOIN (select orderId,SUM(transferCount) AS transferCount from PLM_WH_Transfer_OrderBind group by orderId) B";
            //sqlCmd += " ON A.TradeNo=B.orderId LEFT JOIN PLM_Product_online C ON C.id=A.TradeNo WHERE A.TradeNo='" + orderId + "'";
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "offLine_OrderDetail";
            return dt;
            
        }

        /// <summary>
        /// 生成一个篓车记录id
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int Transfer_offLine_createTranfer(string loginClass,string loginGroup) 
        {
            int transferId = 0;
            string sqlCmd = "insert into PLM_WH_Transfer (TransferTime,bindStatus,bindClss,bindGroup)";
            sqlCmd += "values ('" + DateTime.Now + "','1','" + loginClass + "','" + loginGroup + "')";
            if (SqlSel.ExeSql(sqlCmd) > 0) 
            {
                sqlCmd = "select max(id) from PLM_WH_Transfer";
                transferId = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));                
            }
            return transferId;
        }

        /// <summary>
        /// 记录篓车绑定的订单信息
        /// </summary>
        /// <param name="transferId">篓车记录id</param>
        /// <param name="orderId">生产订单id</param>
        /// <param name="bindNumb">配送数量</param>
        [WebMethod]
        public void Transfer_offLine_orderBind(int transferId, string orderId, string bindNumb) 
        {
            string sqlCmd = "insert into PLM_WH_Transfer_OrderBind (TransferId,orderId,transferCount)";
            sqlCmd += "values ('" + transferId + "','" + orderId + "','" + bindNumb + "')";
            SqlSel.ExeSql(sqlCmd);
        }

        /// <summary>
        /// 推车加工情况
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <param name="TreatType">加工类型</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable Treat_orderInfo(string barCode, string TreatType)
        {
            DataTable orderInfo = new DataTable();
            string sqlCmd = "select A.*,B.orderNo,B.ItemNo,B.ItemParm,C.RecordId,isnull(C.TreatStatus,'-1') as TreatStatus,isnull(D.StorageCode,'0') as StorageCode ";
            sqlCmd+=" from PLM_Serials_BindBarCode A left join PLM_Product_OnLine B";
            sqlCmd += " ON A.tradeNo=B.id left join PLM_WH_TreatRecord_B C ON C.RecordId=A.id and C.TreatType='" + TreatType + "'";
            sqlCmd += " left join PLM_WH_Storage_Actual D ON D.CntrCode=A.BarCode";
            sqlCmd += " where A.BarCode='" + barCode + "' and A.OlineStatus='1'";
            SqlSel.GetSqlSel(ref orderInfo, sqlCmd);
            orderInfo.TableName = "Treat_orderInfo";
            return orderInfo;
        }

        /// <summary>
        /// 推车加工记录
        /// </summary>
        /// <param name="RecordId">上架流水号</param>
        /// <param name="TreatClass">班次</param>
        /// <param name="TreatGroup">班组</param>
        /// <param name="TreatType">加工类型</param>
        /// <param name="TreatStatus">加工状态</param>
        /// <returns></returns>
        [WebMethod]
        public int Treat_optRecord(string RecordId, string TreatClass, string TreatGroup, string TreatType, string TreatStatus) 
        {
            string sqlCmd = "";
            if (TreatStatus == "1") 
            {
                sqlCmd = "insert into PLM_WH_TreatRecord_B (RecordId,TreatTime,TreatClass,TreatGroup,TreatType,TreatStatus) values";
                sqlCmd += "('" + RecordId + "','" + DateTime.Now + "','" + TreatClass + "','" + TreatGroup + "','" + TreatType + "','1')";
            }
            if (TreatStatus == "0") 
            {
                sqlCmd = "update PLM_WH_TreatRecord_B SET TreatTime='" + DateTime.Now + "',TreatStatus='0' where RecordId='" + RecordId + "' and TreatType='" + TreatType + "'";
            }

            int execCount = SqlSel.ExeSql(sqlCmd);
            return execCount;
        }

        /// <summary>
        /// 剪边入库
        /// </summary>
        /// <param name="recordId">生产流水id</param>
        /// <param name="origStorageCode">原先的库位码</param>
        /// <param name="curStorageCode">当前扫描的库位码</param>
        /// <param name="treatType">1:剪边</param>
        /// <returns>1:成功入库;-1:重复入库;0:未成功入库</returns>
        [WebMethod]
        public int Treat_reBoundStorage(string recordId, string origStorageCode, string curStorageCode,string treatType)
        {
            string sqlCmd = "select * from PLM_WH_TreatRecord_B left join PLM_Serials_BindBarCode B on B.id=PLM_WH_TreatRecord_B.recordId";
            sqlCmd+=" where RecordId='" + recordId + "' and TreatType='" + treatType + "' and TreatStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                string onlineId = dt.Rows[0]["tradeNo"].ToString();
                string barCode = dt.Rows[0]["BarCode"].ToString();
                string changeNumb = dt.Rows[0]["BindQty"].ToString();

                if (origStorageCode == "0")//未入库直接入库
                {
                    //插入收货确认明细
                    WH_Serials_Bound(onlineId, changeNumb);
                    //更新库存
                    Storage_BoundState(barCode, curStorageCode, 'B' + recordId, "1", onlineId, changeNumb);
                }
                else 
                {
                    //若库位不同则需先进行转储动作
                    if (curStorageCode != origStorageCode)
                    {
                        WH_ChangeStorage(barCode, origStorageCode, curStorageCode, changeNumb, recordId, onlineId);
                    }
                }

                //完成加工状态
                sqlCmd = "update PLM_WH_TreatRecord_B set TreatStatus='0',TreatTime='" + DateTime.Now + "'";
                sqlCmd += " where RecordId='" + recordId + "' and TreatType='" + treatType + "'";
                if (SqlSel.ExeSql(sqlCmd) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
                
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 库位转储
        /// </summary>
        /// <param name="barCode">货架条码</param>
        /// <param name="origStorageCode">转出库位码</param>
        /// <param name="curStorageCode">转入库位码</param>
        /// <param name="changeNumb">数量</param>
        /// <param name="recordId">生产流水id</param>
        /// <param name="onlineId">生产订单id</param>
        [WebMethod]
        public void WH_ChangeStorage(string barCode, string origStorageCode, string curStorageCode,string changeNumb,string recordId,string onlineId) 
        {
            //先执行出库
            Storage_BoundState(barCode, origStorageCode, "", "0", onlineId, changeNumb);
            //再入库
            Storage_BoundState(barCode, curStorageCode, "", "1", onlineId, changeNumb);
        }

        /// <summary>
        /// 完成加工后返还原库位的操作
        /// </summary>
        /// <param name="recordId">上架流水id</param>
        /// <param name="treatType">加工类型</param>
        /// <returns></returns>
        [WebMethod]
        public int Treat_returnStorage(string recordId, string treatType) 
        {
            string sqlCmd = "select * from PLM_WH_TreatRecord_B where RecordId='" + recordId + "' and TreatType='" + treatType + "' and TreatStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count > 0)
            {
                //直接完成加工状态
                sqlCmd = "update PLM_WH_TreatRecord_B set TreatStatus='0',TreatTime='" + DateTime.Now + "'";
                sqlCmd += " where RecordId='" + recordId + "' and TreatType='" + treatType + "'";
                if (SqlSel.ExeSql(sqlCmd) > 0)
                {
                    return 1;
                }
                else 
                {
                    return 0;
                }
            }
            else 
            {
                return 1;
            }
        }

        /// <summary>
        /// 获取配送数据清单
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataTable getDistData() 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select A.itemNo,B.StorageCode,distCount,CntrCode,D.ItemParm,A.id,D.OrderNo,A.distLine from PLM_WH_Distribution_Temp A";
            sqlCmd += " left join PLM_WH_Storage_Actual B ON A.strorageId =B.ID";
            sqlCmd += " LEFT JOIN PLM_Product_OnLine D ON B.OnlineId=D.Id WHERE A.isTemp='0' and scatSign='0'";
            sqlCmd += " union select t1.itemNo,'零散区' as StorageCode,distCount,BarCode,t3.ItemParm,t1.Id,t3.OrderNo,t1.distLine from PLM_WH_Distribution_Temp t1";
            sqlCmd += " left join PLM_Serials_BindBarCode t2 on t1.strorageId=t2.id";
            sqlCmd += " LEFT JOIN PLM_Product_OnLine t3 ON t3.id=t2.TradeNo WHERE t1.isTemp='0' and t1.scatSign='1'";
            //string sqlCmd = "select * from PLM_WH_Distribution_Temp where isTemp='0'";
            //if (SqlSel.GetSqlSel(ref dt, sqlCmd)) 
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++) 
            //    {

            //    }
            //}
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "DistData";
            return dt;
        }

        /// <summary>
        /// 根据配送车流水码进行出库操作
        /// </summary>
        /// <param name="tempDistId">配送流水码数组</param>
        [WebMethod]
        public int execDist(string[] tempDistId)
        {
            int execLines = 0;
            foreach (string tempId in tempDistId)
            {
                string StorageCode = "", CntrCode = "", onlineId = "", distCount = "", distLine = "";
                string sqlCmd = "select * from PLM_WH_Distribution_Temp where id='" + tempId + "'";
                DataTable dt = new DataTable();
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    execLines += 1;
                    DataTable tempDt = new DataTable();
                    distCount = dt.Rows[0]["distCount"].ToString();
                    distLine = dt.Rows[0]["distLine"].ToString();
                    string storageId = dt.Rows[0]["strorageId"].ToString();//入库流水id
                    //在库货架
                    if (dt.Rows[0]["scatSign"].ToString() == "0")
                    {
                        //库位解绑
                        sqlCmd = "update PLM_WH_Storage_Actual set isLocked='0' where id='" + storageId + "'";
                        if (SqlSel.ExeSql(sqlCmd) == 1)
                        {
                            sqlCmd = "select * from PLM_WH_Storage_Actual where id='" + storageId + "'";
                            SqlSel.GetSqlSel(ref tempDt, sqlCmd);
                            StorageCode = tempDt.Rows[0]["StorageCode"].ToString();
                            CntrCode = tempDt.Rows[0]["CntrCode"].ToString();
                            onlineId = tempDt.Rows[0]["onlineId"].ToString();
                            string recordId = "";
                            sqlCmd = "select ('B'+ CONVERT(NVARCHAR(50),Id)) as recordId,* from PLM_Serials_BindBarCode where barCode='" + CntrCode + "' and OlineStatus='1'";
                            DataTable bindInfo = new DataTable();
                            if (SqlSel.GetSqlSel(ref bindInfo, sqlCmd))
                            {
                                recordId = bindInfo.Rows[0]["recordId"].ToString();
                            }
                            //执行出库
                            Storage_BoundState(CntrCode, StorageCode, recordId, "0", onlineId, distCount);

                            //生成配送信息
                            sqlCmd = "insert into PLM_WH_Distribution_List (sourceNo,distTime,ContCode,PlanDistQty,ActualDistQty,StorageCode,onlineId,distLine)";
                            sqlCmd += " values ('" + dt.Rows[0]["SourceNo"].ToString() + "','" + DateTime.Now + "','" + CntrCode + "','" + distCount + "',";
                            sqlCmd += "'" + distCount + "','" + StorageCode + "','" + onlineId + "','" + distLine + "')";
                            if (SqlSel.ExeSql(sqlCmd) > 0)
                            {
                                //删除配送车数据
                                sqlCmd = "delete from PLM_WH_Distribution_Temp where id='" + tempId + "'";
                                SqlSel.ExeSql(sqlCmd);
                            }
                            else 
                            {
                                execLines -= 1;
                            }
                        }
                        else 
                        {
                            execLines -= 1;
                        }
                    }
                    if (dt.Rows[0]["scatSign"].ToString() == "1") //零散区出库
                    {
                        sqlCmd = "select * from PLM_Serials_BindBarCode where id='" + storageId + "'";
                        SqlSel.GetSqlSel(ref tempDt, sqlCmd);
                        CntrCode = tempDt.Rows[0]["BarCode"].ToString();
                        onlineId = tempDt.Rows[0]["tradeNo"].ToString();
                        //生成配送信息
                        sqlCmd = "insert into PLM_WH_Distribution_List (sourceNo,distTime,ContCode,PlanDistQty,ActualDistQty,StorageCode,onlineId)";
                        sqlCmd += " values ('" + dt.Rows[0]["SourceNo"].ToString() + "','" + DateTime.Now + "','" + CntrCode + "','" + distCount + "',";
                        sqlCmd += "'" + distCount + "','零散区','" + onlineId + "')";
                        if (SqlSel.ExeSql(sqlCmd) > 0)
                        {
                            //删除配送车数据
                            sqlCmd = "delete from PLM_WH_Distribution_Temp where id='" + tempId + "'";
                            SqlSel.ExeSql(sqlCmd);

                            //清除货架绑定状态
                            sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0',DistLocked='0' where id='" + storageId + "'";
                            SqlSel.ExeSql(sqlCmd);
                        }
                        else 
                        {
                            execLines -= 1;
                        }
                    }
                }
            }
            return execLines;//返回最终执行行数
        }

        /// <summary>
        /// 根据货架或篓车条码查询库位订单信息
        /// </summary>
        /// <param name="barCode">扫描条码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable GetOrderInfoFromStorage(string barCode)
        {
            DataTable dt = new DataTable();
            if (barCode != "0000") 
            {
                string sqlCmd = "select A.id as storageId,* from PLM_WH_Storage_Actual A left join PLM_Product_OnLine B on A.OnlineId=B.id where CntrCode='" + barCode + "'";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                dt.TableName = "orderInfo_cntr";
            }
            return dt;
        }

        /// <summary>
        /// 合并货架或篓车操作
        /// </summary>
        /// <param name="storageId">需合并的入库流水id</param>
        /// <param name="targetCntr">合并后的货架或篓车条码</param>
        /// <returns></returns>
        [WebMethod]
        public bool WH_Storage_MergeCntr(string storageId,string targetCntr) 
        {
            string sqlCmd = "select * from PLM_WH_Storage_Actual where id='" + storageId + "'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                string cntrCode = dt.Rows[0]["CntrCode"].ToString();
                string storageCode = dt.Rows[0]["StorageCode"].ToString();
                string onlineId = dt.Rows[0]["OnlineId"].ToString();
                string boundQty = dt.Rows[0]["BoundQty"].ToString();
                string boundTime = dt.Rows[0]["BoundTime"].ToString();
                //更新库存
                sqlCmd = "select * from PLM_WH_Storage_Actual where CntrCode='" + targetCntr + "'";
                DataTable tgtDt = new DataTable();
                SqlSel.GetSqlSel(ref tgtDt, sqlCmd);
                //查找生产来源id
                string recordId = "";
                sqlCmd = "select ('B' + CONVERT(nvarchar(50),id)) as recordId,* from PLM_Serials_BindBarCode where barCode='" + cntrCode + "' and OlineStatus='1'";
                DataTable bindInfo = new DataTable();
                if (SqlSel.GetSqlSel(ref bindInfo, sqlCmd)) 
                {
                    recordId = bindInfo.Rows[0]["recordId"].ToString();
                }
                //执行出库
                Storage_BoundState(cntrCode, storageCode, recordId, "0", onlineId, boundQty);
                //同一订单则合并
                DataRow[] dr = tgtDt.Select("OnlineId='" + onlineId + "'");
                if (dr.Length == 1)
                {
                    sqlCmd = "update PLM_WH_Storage_Actual set BoundQty=BoundQty +" + boundQty + " where id='" + dr[0]["id"].ToString() + "'";
                }
                else
                {
                    sqlCmd = "insert into PLM_WH_Storage_Actual (StorageCode,OnlineId,CntrCode,BoundQty,BoundTime) values ('" + dr[0]["StorageCode"].ToString() + "',";
                    sqlCmd += "'" + onlineId + "','" + targetCntr + "','" + boundQty + "','" + boundTime + "')";
                }

                if (SqlSel.ExeSql(sqlCmd) > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// 仓库收货流水（入库单）
        /// </summary>
        /// <param name="onlineId">生产订单id</param>
        /// <param name="boundQty">收货数量</param>
        [WebMethod]
        public void WH_Serials_Bound(string onlineId, string boundQty) 
        {
            string sqlCmd = "insert into PLM_WH_Serilas_Bound (OnlineId,BoundQty,BoundTime) values";
            sqlCmd += "('" + onlineId + "','" + boundQty + "','" + DateTime.Now + "')";
            SqlSel.ExeSql(sqlCmd);
        }

        /// <summary>
        /// 数量盘点
        /// </summary>
        /// <param name="storageId">库存流水id</param>
        /// <param name="verifyCount">盘点数量</param>
        /// <returns></returns>
        [WebMethod]
        public int WH_Verify_Count(string storageId,string verifyCount)
        {
            string sqlCmd = "select * from PLM_WH_Storage_Actual where id='" + storageId + "'";
            DataTable dt = new DataTable();
            int execCount = 0;
            if (SqlSel.GetSqlSel(ref dt,sqlCmd))
            {
                string cntrCode = dt.Rows[0]["CntrCode"].ToString();
                string storageCode = dt.Rows[0]["StorageCode"].ToString();
                string onlineId = dt.Rows[0]["OnlineId"].ToString();
                string boundQty = dt.Rows[0]["BoundQty"].ToString();
                string boundTime = dt.Rows[0]["BoundTime"].ToString();

                sqlCmd = "insert into [PLM_WH_InBound_Record] (BarCode,StorageCode,RecordId,BoundStatus,onLineId,BoundQty,boundTime)";
                sqlCmd += " values ('" + cntrCode + "','" + storageCode + "','" + storageId + "','2','" + onlineId + "',";
                sqlCmd += "'" + boundQty + "','" + DateTime.Now + "')";
                if (SqlSel.ExeSql(sqlCmd) > 0)
                {
                    sqlCmd = "update PLM_WH_Storage_Actual set BoundQty='" + verifyCount + "' where id='" + storageId + "'";
                    execCount = SqlSel.ExeSql(sqlCmd);
                }
            }
            return execCount;
        }

        /// <summary>
        /// 货架转储篓车的操作
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="storageArr"></param>
        /// <returns></returns>
        [WebMethod]
        public int WH_Storage_b2t(ref DataSet ds, string[] storageArr,string storageCode)
        {
            //操作生产订单列表
            DataTable orderDt = new DataTable();
            orderDt.Columns.Add("ItemNo");
            orderDt.Columns.Add("ItemParm");
            orderDt.Columns.Add("tsCount");
            orderDt.Columns.Add("PlanSum");
            orderDt.Columns.Add("scddcp_jhpch");
            
            //操作的订单列表
            DataTable prodDt = new DataTable();
            prodDt.Columns.Add("onlineId");
            prodDt.Columns.Add("ItemNo");
            prodDt.Columns.Add("OrderNo");
            prodDt.Columns.Add("ItemParm");
            prodDt.Columns.Add("PlanCount");
            prodDt.Columns.Add("tQty");
            prodDt.Columns.Add("scddcp_jhpch");
            //解析操作的入库流水及数量
            DataTable storageDt = new DataTable();
            storageDt.Columns.Add("storageId");
            storageDt.Columns.Add("optQty");
            foreach (string str in storageArr) 
            {
                DataRow dr = storageDt.NewRow();
                string storageId = str.Substring(0, str.IndexOf("/"));
                string optQty = str.Substring(str.IndexOf("/") + 1);
                dr["storageId"] = storageId;
                dr["optQty"] = optQty;
                string sqlStr = "select ItemNo,ItemParm,ItemTech,OrderNo,boundQty,OnlineId,PlanCount,";
                sqlStr += " (select TOP 1 scddcp_jhpch from PLM_Product_Rel WHERE ProdId=OnlineId) as scddcp_jhpch from ";
                sqlStr += " PLM_WH_Storage_Actual t1 left join PLM_Product_OnLine t2 on t2.Id=t1.OnlineId";
                sqlStr += " where t1.id='" + storageId + "'";
                DataTable tempDt = new DataTable();
                SqlSel.GetSqlSel(ref tempDt, sqlStr);
                storageDt.Rows.Add(dr);

                //判断onlineId是否存在
                if (prodDt.Rows.Count > 0)
                {
                    int find = BasePage.tableValueIndex(prodDt, "onlineId", tempDt.Rows[0]["OnlineId"].ToString());
                    if (find >= 0)
                    {
                        prodDt.Rows[find]["tQty"] = Convert.ToInt32(optQty) + Convert.ToInt32(prodDt.Rows[find]["tQty"]);
                    }
                    else 
                    {
                        DataRow prodDr = prodDt.NewRow();
                        prodDr["onlineId"] = tempDt.Rows[0]["OnlineId"];
                        prodDr["ItemNo"] = tempDt.Rows[0]["ItemNo"];
                        prodDr["OrderNo"] = tempDt.Rows[0]["OrderNo"];
                        prodDr["ItemParm"] = tempDt.Rows[0]["ItemParm"];
                        prodDr["PlanCount"] = tempDt.Rows[0]["PlanCount"];
                        prodDr["tQty"] = optQty;
                        prodDr["scddcp_jhpch"] = tempDt.Rows[0]["scddcp_jhpch"];
                        prodDt.Rows.Add(prodDr);
                    }
                }
                else 
                {
                    DataRow prodDr = prodDt.NewRow();
                    prodDr["onlineId"] = tempDt.Rows[0]["OnlineId"].ToString();
                    prodDr["ItemNo"] = tempDt.Rows[0]["ItemNo"];
                    prodDr["OrderNo"] = tempDt.Rows[0]["OrderNo"];
                    prodDr["ItemParm"] = tempDt.Rows[0]["ItemParm"];
                    prodDr["PlanCount"] = tempDt.Rows[0]["PlanCount"];
                    prodDr["tQty"] = optQty;
                    prodDr["scddcp_jhpch"] = tempDt.Rows[0]["scddcp_jhpch"];
                    prodDt.Rows.Add(prodDr);
                }
            }

            foreach (DataRow dr in prodDt.Rows) 
            {
                if (orderDt.Rows.Count > 0)
                {
                    int find = BasePage.tableValueIndex(orderDt, "scddcp_jhpch", dr["scddcp_jhpch"].ToString());
                    if (find >= 0)
                    {
                        orderDt.Rows[find]["tsCount"] = Convert.ToInt32(dr["tQty"]) + Convert.ToInt32(orderDt.Rows[find]["tsCount"]);
                        orderDt.Rows[find]["PlanSum"] = Convert.ToInt32(dr["PlanCount"]) + Convert.ToInt32(orderDt.Rows[find]["PlanSum"]);
                    }
                    else
                    {
                        DataRow orderNr = orderDt.NewRow();
                        orderNr["scddcp_jhpch"] = dr["scddcp_jhpch"];
                        orderNr["ItemNo"] = dr["ItemNo"];
                        orderNr["ItemParm"] = dr["ItemParm"];
                        orderNr["PlanSum"] = dr["PlanCount"];
                        orderNr["tsCount"] = dr["tQty"];
                        orderDt.Rows.Add(orderNr);
                    }
                }
                else
                {
                    DataRow orderNr = orderDt.NewRow();
                    orderNr["scddcp_jhpch"] = dr["scddcp_jhpch"];
                    orderNr["ItemNo"] = dr["ItemNo"];
                    orderNr["ItemParm"] = dr["ItemParm"];
                    orderNr["PlanSum"] = dr["PlanCount"];
                    orderNr["tsCount"] = dr["tQty"];
                    orderDt.Rows.Add(orderNr);
                }
            }
            //string storageStr = string.Join(",", storageArr);
            //string sqlStr = "select ItemNo,ItemParm,ItemTech,OrderNo,boundQty,OnlineId from (";
            //sqlStr += "select sum(boundQty) as boundQty,onlineId from PLM_WH_Storage_Actual where id in (" + storageStr + ") group by onlineId) t1";
            //sqlStr += " left join PLM_Product_OnLine t2 on t2.Id=t1.OnlineId";
            //DataTable orderDt = new DataTable();
            //SqlSel.GetSqlSel(ref orderDt, sqlStr);
            orderDt.TableName = "subOrderTable";
            int tranferId = 0;
            tranferId = Transfer_offLine_createTranfer("", "");//申请一个篓车id
            for (int i = 0; i < prodDt.Rows.Count; i++)
            {
                //插入篓车订单明细
                Transfer_offLine_orderBind(tranferId, prodDt.Rows[i]["OnlineId"].ToString(), prodDt.Rows[i]["tQty"].ToString());

                //执行入库
                Storage_BoundState("T" + tranferId, storageCode, "", "1", prodDt.Rows[i]["OnlineId"].ToString(), prodDt.Rows[i]["tQty"].ToString());
            }

            //原货架执行出库
            foreach (DataRow dr in storageDt.Rows )
            {
                string sqlCmd = "select * from PLM_WH_Storage_Actual where id='" + dr["storageId"] + "'";
                DataTable dt = new DataTable();
                if (SqlSel.GetSqlSel(ref dt, sqlCmd))
                {
                    string cntrCode = dt.Rows[0]["CntrCode"].ToString();
                    string origStorageCode = dt.Rows[0]["StorageCode"].ToString();
                    string onlineId = dt.Rows[0]["OnlineId"].ToString();
                    string boundQty = dr["optQty"].ToString();
                    string boundTime = dt.Rows[0]["BoundTime"].ToString();
                    //查找生产来源id
                    string recordId = "";
                    sqlCmd = "select ('B'+ CONVERT(NVARCHAR(50),Id)) as recordId,* from PLM_Serials_BindBarCode where barCode='" + cntrCode + "' and OlineStatus='1'";
                    DataTable bindInfo = new DataTable();
                    if (SqlSel.GetSqlSel(ref bindInfo, sqlCmd))
                    {
                        recordId = bindInfo.Rows[0]["recordId"].ToString();
                        //sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where id='" + recordId + "'";
                        //if (SqlSel.ExeSql(sqlCmd) > 0)
                        //{
                        //    sqlCmd = "delete from PLM_WH_Storage_Actual where id='" + storageId + "'";
                        //    SqlSel.ExeSql(sqlCmd);
                        //}
                    }
                    Storage_BoundState(cntrCode, origStorageCode, recordId, "0", onlineId, boundQty);
                }
            }

            //篓车入库

            ds.Tables.Add(orderDt);

            return tranferId;
        }

        /// <summary>
        /// 盘点-订单查询
        /// </summary>
        /// <param name="onlineId">生产订单id</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_Check_AddTransfer(string onlineId) 
        {
            string sqlCmd = "SELECT TOP 1 T3.SCDDCP_JHPCH AS orderNo,T3.SCDDCP_JHTCSL,ProdId AS TradeNo,";
            sqlCmd += " T1.orderid,T2.ItemNo,T2.ItemParm,T4.BoundQty AS transferCount";
            sqlCmd += " FROM PLM_Product_Rel T1 LEFT JOIN PLM_Product_OnLine T2 ON T1.ProdId=T2.Id";
            sqlCmd += " LEFT JOIN (SELECT SUM(BoundQty) AS BoundQty,OnlineId FROM PLM_WH_Storage_Actual";
            sqlCmd += " GROUP BY OnlineId) T4 ON T4.OnlineId=T1.ProdId";
            sqlCmd += " LEFT JOIN View_ERP_SCDDCP T3 ON T1.OrderId=T3.SCDDCP_LSBH WHERE ProdId='" + onlineId + "'";
            //string sqlCmd = "select A.TradeNo,transferCount,C.orderNo,C.ItemParm,C.ItemNo,C.ItemName from View_PLM_ProducedCount A";
            //sqlCmd += " LEFT JOIN (select orderId,SUM(transferCount) AS transferCount from PLM_WH_Transfer_OrderBind group by orderId) B";
            //sqlCmd += " ON A.TradeNo=B.orderId LEFT JOIN PLM_Product_online C ON C.id=A.TradeNo WHERE A.TradeNo='" + onlineId + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            dt.TableName = "Check_OrderInfo";
            return dt;
        }

        /// <summary>
        /// 盘点-返回条码订单信息（绑定及最近绑定的订单）
        /// </summary>
        /// <param name="barCode">货架号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_Check_Add_B(string barCode) 
        {
            string sqlCmd = "select ('B'+ convert(nvarchar(50),A.id)) as recordId,A.*,B.orderNo,B.ItemNo,B.ItemParm ";
            sqlCmd += " from PLM_Serials_BindBarCode A left join PLM_Product_online B on A.TradeNo=B.id";
            sqlCmd += " where BarCode='" + barCode + "' and OlineStatus='1'";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "Check_OrderInfo_B";
                return dt;
            }
            else 
            {
                sqlCmd = "select ('B'+ convert(nvarchar(50),A.id)) as recordId,A.*,B.orderNo,B.ItemNo,B.ItemParm ";
                sqlCmd += " from PLM_Serials_BindBarCode A left join PLM_Product_online B on A.TradeNo=B.id";
                sqlCmd += " where BarCode='" + barCode + "' and A.id=(select max(id) from PLM_Serials_BindBarCode where BarCode='" + barCode + "')";
                SqlSel.GetSqlSel(ref dt, sqlCmd);
                dt.TableName = "Check_OrderInfo_B";
                return dt;
            }
        }

        /// <summary>
        /// 盘点-推车入库并更新条码绑定状态
        /// </summary>
        /// <param name="recordId">上线绑定流水</param>
        /// <param name="storageCode">货位条码</param>
        /// <param name="barCode">货架条码</param>
        /// <param name="onlineId">生产订单流水</param>
        /// <param name="BoundQty">入库数量</param>
        /// <param name="onlineStatus">上架绑定状态</param>
        [WebMethod]
        public void WH_Check_BoundStorage_B(string recordId,string storageCode, string barCode, string onlineId, string BoundQty,string onlineStatus)
        {
            //插入收货流水（入库单确认流水）
            //WH_Serials_Bound(onlineId, BoundQty);

            //执行入库
            Storage_BoundState(barCode, storageCode, recordId, "1", onlineId, BoundQty);

            //如果货架只是处于在库但已解绑定状态，则盘点时更新为绑定状态
            if (!string.IsNullOrEmpty(recordId) & onlineStatus == "0") 
            {
                string sqlCmd = "update PLM_Serials_BindBarCode set olineStatus='1' where id='" + recordId.Substring(1) + "'";
                SqlSel.ExeSql(sqlCmd);
            }
        }

        /// <summary>
        /// 根据关键字查询相关订单列表
        /// </summary>
        /// <param name="orderStr">订单号关键字</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_queryOrderList(string orderStr) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select scddcp_jhpch from PLM_Product_Rel where scddcp_jhpch like '%" + orderStr + "%' GROUP BY scddcp_jhpch";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "WH_queryOrderList";
                return dt;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 返回订单货位分布情况
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_OrderStorageList(string orderNo) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select t2.OrderNo,t1.*,t2.ItemNo,t2.ItemParm from PLM_WH_Storage_Actual t1 left join PLM_Product_OnLine t2 on t1.OnlineId=t2.Id";
            sqlCmd += " WHERE t1.OnlineId IN (select ProdId from PLM_Product_Rel where scddcp_jhpch='" + orderNo + "') ORDER BY t2.ItemNo,t1.StorageCode";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "OrderStorageList";
                return dt;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 查询当前库位的订单信息
        /// </summary>
        /// <param name="storageCode">库位码</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_StorageDetails(string storageCode) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select t2.OrderNo,t1.*,t2.ItemNo,t2.ItemParm from PLM_WH_Storage_Actual t1 left join PLM_Product_OnLine t2";
            sqlCmd += " on t1.OnlineId=t2.Id WHERE t1.StorageCode='" + storageCode + "' ORDER BY t2.ItemNo,t1.StorageCode";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "OrderStorageList";
                return dt;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 吊挂线自动计件并解除绑定信息
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <param name="LoginClass">加工班次</param>
        /// <param name="loginGroup">班组</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_TreatAndOffLine(string barCode,string LoginClass,string loginGroup) 
        {
            string sqlCmd = "select A.*,B.ItemParm,B.OrderNo from PLM_Serials_BindBarCode A LEFT JOIN ";
            sqlCmd += " PLM_Product_OnLine B ON A.TradeNo=B.id where BarCode='" + barCode + "' and OlineStatus='1'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            if (dt.Rows.Count == 1)
            {
                string serialsId = dt.Rows[0]["id"].ToString();//上线流水id

                //插入剪边记录
                sqlCmd = "insert into PLM_WH_TreatRecord_B (RecordId,TreatTime,TreatClass,TreatGroup,TreatType,TreatStatus) values";
                sqlCmd += "('" + serialsId + "','" + DateTime.Now + "','" + LoginClass + "','" + loginGroup + "','1','0')";
                SqlSel.ExeSql(sqlCmd);
                //Treat_optRecord(serialsId, LoginClass, loginGroup, "1", "1");

                //更改上线状态
                sqlCmd = "update PLM_Serials_BindBarCode set OlineStatus='0' where id='" + serialsId + "'";
                SqlSel.ExeSql(sqlCmd);

                //插入下线清单
                sqlCmd = "insert into PLM_Product_OffLine (onlineId,optUser,optTime)";
                sqlCmd += " values ('" + serialsId + "','" + loginGroup + "','" + DateTime.Now + "')";
                SqlSel.ExeSql(sqlCmd);

                dt.TableName = "orderInfo_flowLine";

                return dt;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 根据订单号查询相关物料及生产计划信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_orderQueryByOrderNo(string orderNo) 
        {
            string sqlCmd = "SELECT T2.SCDDCP_LSBH,T2.SCDDCP_JHTCSL,T2.SCDDCP_WLBH,T2.SCDDCP_JHPCH,T3.LSWLZD_GGXH,T4.onlineId FROM ";
            sqlCmd += " (SELECT OrderId FROM PLM_Product_Rel WHERE scddcp_jhpch='" + orderNo + "'";
            sqlCmd += " GROUP BY OrderId) T1 LEFT JOIN View_ERP_SCDDCP T2 ON T1.OrderId=T2.SCDDCP_LSBH";
            sqlCmd += " LEFT JOIN View_LSWLZD T3 ON T3.LSWLZD_WLBH=T2.SCDDCP_WLBH";
            sqlCmd += " LEFT JOIN (SELECT OrderId,MIN(ProdId) AS onlineId FROM PLM_Product_Rel GROUP BY OrderId) T4 ";
            sqlCmd += " ON T4.OrderId=T2.SCDDCP_LSBH";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "orderItemInfo";
                return dt;
            }
            else 
            {
                return null;
            }
            
        }

        /// <summary>
        /// 获取配送产线信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_getDistLines() 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select * from PLM_WH_Distribution_Lines where InUse='1'";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "DistLines";
                return dt;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 判断上架条码跟上架条码是否符合规则
        /// </summary>
        /// <param name="subCount">上架数量</param>
        /// <param name="barCode">条码号</param>
        /// <returns></returns>
        [WebMethod]
        public bool PROD_IsValiadBarCode(int subCount, string barCode) 
        {
            try
            {
                bool result = true;
                if (barCode.StartsWith("B"))
                {
                    //大架子上架数量最多为300个
                    if (subCount > 300)
                    {
                        result = false;
                    }
                    //条码范围为B1000-B10000
                    int numb = Convert.ToInt32(barCode.Substring(1));
                    if (numb > 10000 || numb < 1000)
                    {
                        result = false;
                    }

                }
                else
                {
                    //排除以0开头的情况
                    if (barCode.StartsWith("0"))
                    {
                        result = false;
                    }
                    else
                    {
                        int sBarCode = Convert.ToInt32(barCode);
                        //条码范围为1000-10000
                        if (sBarCode > 10000 || sBarCode < 1000)
                        {
                            result = false;
                        }
                        //吊挂上架数量最多不得大于20个
                        if (subCount > 20)
                        {
                            result = false;
                        }
                    }


                }

                return result;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 获取前n单未完成加工的订单信息
        /// </summary>
        /// <param name="topNumb"></param>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_TreatProgress(int topNumb) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "SELECT t1.*,t2.BoundTotal,((t2.BoundTotal-t1.RemainTreated)*1.0/t2.BoundTotal) AS process FROM (";
            sqlCmd += " SELECT TOP " + topNumb + " scddcp_jhpch AS orderNo,";
            sqlCmd += " SUM(BoundQty) AS RemainTreated,MIN(ProdTime) AS EarlistProdTime ";
            sqlCmd += " FROM View_PLM_StorageDetails WHERE CntrCode LIKE 'B%'";
            sqlCmd += " AND Treated<0 GROUP BY scddcp_jhpch ORDER BY EarlistProdTime";
            sqlCmd += " ) t1 LEFT JOIN ";
            sqlCmd += " (SELECT scddcp_jhpch,SUM(BoundQty) AS BoundTotal FROM View_PLM_StorageDetails GROUP BY scddcp_jhpch) t2";
            sqlCmd += " ON T1.orderNo=t2.scddcp_jhpch";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "TreatProgress";
                return dt;
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 所有库位信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataTable WH_AllStorageInfo() 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "SELECT scddcp_jhpch,StorageCode FROM View_PLM_StorageDetails ";
            sqlCmd += "WHERE StorageCode LIKE 'W%' OR StorageCode LIKE 'E%' GROUP BY scddcp_jhpch,StorageCode";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "AllStorageInfo";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取条码是否存在订单信息及实时上架总数
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [WebMethod]
        public DataTable PROD_BarCodeInfo(string barCode) 
        {
            string sqlStr = "select top 1 PLM_Serials_BindBarCode.id as bid,* from PLM_Serials_BindBarCode left join PLM_Product_OnLine on tradeno=PLM_Product_OnLine.id ";
            sqlStr += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=PLM_Serials_BindBarCode.tradeno";
            sqlStr += " where barcode='" + barCode + "' and olineStatus=1";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlStr))
            {
                dt.TableName = "bindInfo";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 执行上架
        /// </summary>
        /// <param name="BarCode">架号</param>
        /// <param name="TradeNo">工单Id</param>
        /// <param name="BindQty">上架数量</param>
        /// <param name="ExcUser">班次</param>
        /// <param name="lineId">流水线Id</param>
        /// <returns>返回执行结果</returns>
        [WebMethod]
        public string PROD_OrderInfoBind(string BarCode, string TradeNo, string BindQty, string ExcUser, string lineId)
        {
            DateTime optTime = DateTime.Now;
            //生产日期为周日的自动减一天
            //if (optTime.DayOfWeek == DayOfWeek.Sunday) 
            //{
            //    optTime = optTime.AddDays(-1);
            //}
            string sqlCmd = "insert into PLM_Serials_BindBarCode (BarCode,TradeNo,CreateTime,BindQty,ExcUser";
            sqlCmd += ",OlineStatus,lineId) values ('" + BarCode + "','" + TradeNo + "','" + optTime + "',";
            sqlCmd += "'" + BindQty + "','" + ExcUser + "','1','" + lineId + "')";
            string returnInfo = "{\"result\":";
            if (SqlSel.ExeSql(sqlCmd) > 0)
            {
                returnInfo += "\"ok\",\"optTime\":\"" + optTime + "\",\"errorMsg\":\"\"";
            }
            else 
            {
                returnInfo += "\"error\",\"optTime\":\"" + optTime + "\",\"errorMsg\":\"上架失败！请重试.\"";
            }

            returnInfo += "}";

            return returnInfo;

        }

        /// <summary>
        /// 修改上架信息
        /// </summary>
        /// <param name="execTime">执行时间</param>
        /// <param name="recvId">工单id</param>
        /// <param name="bindQty">上架数量</param>
        /// <param name="banzhi">班次信息</param>
        /// <param name="lineId">产线id</param>
        /// <param name="sId">上架流水id</param>
        /// <returns></returns>
        [WebMethod]
        public bool PROD_ModifyBindInfo(ref DateTime execTime,string recvId, string bindQty, string banzhi, string lineId, string sId)
        {
            DateTime optTime = DateTime.Now;
            //生产日期为周日的自动减一天
            //if (optTime.DayOfWeek == DayOfWeek.Sunday) 
            //{
            //    optTime = optTime.AddDays(-1);
            //}
            string sqlCmd = "SELECT * FROM PLM_Serials_BindBarCode WHERE Id='" + sId + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            sqlCmd = "update PLM_Serials_BindBarCode set TradeNo='" + recvId + "',CreateTime='" + optTime + "',";
            sqlCmd += "BindQty='" + bindQty + "',ExcUser='" + banzhi + "',lineId='" + lineId + "' where id='" + sId + "'";
            if (SqlSel.ExeSql(sqlCmd) > 0)
            {
                execTime = optTime;
                sqlCmd = "insert into PLM_Serials_ModLog (recordId,orderId,bindCount,OPTime,OPUser) values ";
                sqlCmd += "('" + sId + "','" + dt.Rows[0]["TradeNo"].ToString() + "','" + dt.Rows[0]["BindQty"].ToString() + "',";
                sqlCmd += "'" + dt.Rows[0]["CreateTime"].ToString() + "','" + dt.Rows[0]["ExcUser"].ToString() + "')";
                SqlSel.ExeSql(sqlCmd);//插入相关修改前数据
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取产线工单信息
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable PROD_GetLineOrders(int lineId) 
        {
            DataTable dt = new DataTable();
            string sqlCmd = "select a.Id,a.OrderNo,a.ItemParm,a.PlanCount,a.ItemName,b.ImgURL,ISNULL(c.onlineSum,0) AS onlineSum,";
            sqlCmd += "ISNULL(a.RedLineCount,0) AS RedLineCount,a.PreSetCount,a.itemNo,a.itemTech from PLM_Product_OnLine a ";
            sqlCmd += "left join PLM_Product_Image b on a.id=b.OnlineId ";
            sqlCmd += "left join (select sum(bindQty) as onlineSum,tradeno from PLM_Serials_BindBarCode group by tradeno) c on c.tradeno=a.id";
            sqlCmd += " where onlineStatus=1 and LineId='" + lineId + "'";
            sqlCmd += " order by BuildTime desc";
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                dt.TableName = "lineOrders";
                return dt;
            }
            else 
            {
                return null;
            }
            
        }

        /// <summary>
        /// 退出登录后清空产线登录信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [WebMethod]
        public bool PROD_ClearLoginInfo(int lineId) 
        {
            string sqlCmd = "update PLM_Product_Line set curentClass=null where id='" + lineId + "'";
            if (SqlSel.ExeSql(sqlCmd) > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前产线、班组的产量
        /// </summary>
        /// <param name="queryTime">查询时间</param>
        /// <param name="lineId">产线id</param>
        /// <param name="banzhi">班次</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable PROD_BindCount(ref DateTime queryTime, int lineId,string banzhi) 
        {
            string sdt = "";
            DateTime curTime = DateTime.Now;
            if (banzhi == "C")
            {
                sdt = curTime.ToString("yyyy-MM-dd");
            }
            if (banzhi == "D")
            {
                if (curTime.Hour < 12)
                {
                    sdt = curTime.AddDays(-1).ToString("yyyy-MM-dd") + " " + "12:00";//跨天统计天数减一
                }
                if (DateTime.Now.Hour > 12)
                {
                    sdt = curTime.ToString("yyyy-MM-dd") + " " + "12:00";
                }
            }

            string sqlCmd = "select a.bindSum,b.orderno,b.plancount,b.itemNo,b.itemParm,b.endTime,b.itemName,c.bindTotal,(lswlex_c2+'*'+lswlex_c3+'*'+lswlex_c4) as itemParm_size,";
            sqlCmd += "lswlex_c9 as itemParm_weight,lswlex_c5 as itemParm_color,lswlex_c7 as itemParm_sfjz,c.bindTotal";
            sqlCmd += " from (select sum(bindQty) as bindSum,tradeno from PLM_Serials_BindBarCode ";
            sqlCmd += "where lineId like '" + lineId + "' and excUser like '" + banzhi + "'";
            sqlCmd += " and CreateTime>='" + sdt + "' and CreateTime<='" + curTime + "' group by tradeNo) a ";
            sqlCmd += "left join PLM_Product_OnLine b on a.tradeno=b.id";
            sqlCmd += " left join (select sum(bindQty) as bindTotal,tradeno from PLM_Serials_BindBarCode group by tradeNo) c ";
            sqlCmd += " on c.tradeno=a.tradeNo";
            sqlCmd += " left join view_plm_lswlex on lswlex_wlbh=itemNo";
            DataTable dt = new DataTable();
            if (SqlSel.GetSqlSel(ref dt, sqlCmd))
            {
                queryTime = curTime;
                dt.TableName = "BindCount";
                return dt;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 实际生产时间与验厂日历的校验
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [WebMethod]
        public int PROD_isValidDate(DateTime dt)
        {
            string compTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlCmd = "select max(InitDay) as recDay from [PLM_classInit]";
            sqlCmd += " where InitDay <= '" + compTime + "' and CheckFlag='0'";
            DataTable result = new DataTable();
            SqlSel.GetSqlSel(ref result, sqlCmd);
            DateTime recDay = Convert.ToDateTime(result.Rows[0]["recDay"]);
            return -compDays(recDay, dt);
            //if (dt.DayOfWeek == DayOfWeek.Sunday)
            //{
            //    return -1;
            //}
            //else
            //{
            //    return 0;
            //}
        }
        //计算两个时间的天数差，只比较天数部分
        private static int compDays(DateTime d1, DateTime d2)
        {
            DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));
            DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
            int days = (d4 - d3).Days;
            return days;
        }

        /// <summary>
        /// 获取服务器时间并返回与验厂日期差异
        /// </summary>
        /// <param name="serverTime"></param>
        /// <returns></returns>
        [WebMethod]
        public int WH_getServerTime(ref DateTime serverTime)
        {
            serverTime = DateTime.Now;
            return PROD_isValidDate(serverTime);
        }

    }
}
