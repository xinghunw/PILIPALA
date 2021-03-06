﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Configuration;

using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.PostKey;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.com.MySQL;
using WaterLibrary.com.pilipala;



namespace PILIPALA.system.serv
{
    /// <summary>
    /// SysServ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]

    public class SysServ : System.Web.Services.WebService
    {
        public PLSYS PLSYS;
        public PLDR PLDR;
        public PLDU PLDU;

        public SysServ()
        {
            /* 初始化噼里啪啦数据库信息和MySql控制器 */
            PLDB PLDB = new PLDB
            {
                Tables = PLSYS.DefTables,
                Views = PLSYS.DefViews,
                MySqlManager = new MySqlManager()
            };
            PLSYS = new PLSYS(2, PLDB);

            PLSYS.DefaultSysTables();
            PLSYS.DefaultSysViews();

            /* 初始化数据库连接 */
            PLSYS.DBCHINIT(new MySqlConn
            {
                DataSource = WebConfigurationManager.AppSettings["DataSource"],
                DataBase = WebConfigurationManager.AppSettings["DataBase"],
                Port = WebConfigurationManager.AppSettings["Port"],
                User = WebConfigurationManager.AppSettings["User"],
                PWD = WebConfigurationManager.AppSettings["PWD"]
            });

            PLDR = new PLDR(PLSYS);
            PLDU = new PLDU(PLSYS);
        }



        /// <summary>
        /// count_star计数减一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int StarCount_subs(int ID)
        {
            SysServ SysServ = new SysServ();
            int StarCount = SysServ.PLDR.GetIndex(ID).StarCount;

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount - 1);

            return StarCount - 1;
        }
        /// <summary>
        /// count_star计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int StarCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            int StarCount = SysServ.PLDR.GetIndex(ID).StarCount;

            SysServ.PLDU.UpdateIndex<StarCount>(ID, StarCount + 1);

            return StarCount + 1;
        }
        /// <summary>
        /// count_pv计数加一
        /// </summary>
        /// <param name="ID">文章序列号</param>
        [WebMethod]
        public int UVCount_plus(int ID)
        {
            SysServ SysServ = new SysServ();
            int UVCount = SysServ.PLDR.GetIndex(ID).UVCount;

            SysServ.PLDU.UpdateIndex<UVCount>(ID, UVCount + 1);

            return UVCount + 1;
        }

        /// <summary>
        /// 取得内核版本
        /// </summary>
        /// <returns>返回内核版本</returns>
        [WebMethod]
        public string GetCoreVersion()
        {
            return WaterLibrary.Assembly.Version;
        }
    }
}
