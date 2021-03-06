﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.com.MySQL;
using WaterLibrary.stru.MySQL;
using CommentLake.stru;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql;
using System.Data;

namespace CommentLake
{
    public class CommentLake
    {
        #region WL数据库操作标准配置

        public string DataBase { get; private set; }
        public string CommentTable { get; private set; }
        public MySqlManager MySqlManager { get; private set; }

        public CommentLake(string DataBase, string CommentTable)
        {
            this.DataBase = DataBase;
            this.CommentTable = CommentTable;
        }

        public void DBCHINIT(MySqlConn MySqlConn)
        {
            MySqlManager = new MySqlManager();
            MySqlManager.Start(MySqlConn);
        }

        #endregion

        /// <summary>
        /// 得到最大评论ID（私有）
        /// </summary>
        /// <returns>错误则返回-1</returns>
        private int GetMaxCommentID()
        {
            try
            {
                string SQL = string.Format("SELECT max(CommentID) FROM {0}.`{1}`", DataBase, CommentTable);

                return Convert.ToInt32(MySqlManager.GetRow(SQL)[0]);
            }
            catch
            {
                return -1;
            }
        }

        /* 评论删除功能列为第二优先级 */

        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="PostID">评论归属的文章ID</param>
        /// <param name="Comment">评论内容</param>
        /// <param name="HEAD">回复到评论的CommentID，为空则表示为非回复性评论</param>
        /// <returns></returns>
        public bool AddComment(Comment Comment)
        {
            string SQL = string.Format(
                "INSERT INTO {0}.`{1}` " +
                "(`CommentID`, `PostID`, `Name`, `Email`, `Content`, `WebSite`, `HEAD`) VALUES " +
                "(`?CommentID`,`?PostID`,`?Name`,`?Email`,`?Content`,`?WebSite`,`?HEAD`);"

                , DataBase, CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
            {
                new MySqlParm() { Name = "?CommentID", Val = GetMaxCommentID() + 1 },
                new MySqlParm() { Name = "?PostID", Val =  Comment.PostID},
                new MySqlParm() { Name = "?Name", Val =  Comment.Name},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?Content", Val =  Comment.Content},
                new MySqlParm() { Name = "?HEAD", Val =  Comment.HEAD},
            };

            if (MySqlManager.ParmQueryCMD(SQL, ParmList).ExecuteNonQuery() == 2)
                return true;
            else
                throw new Exception("多行操作异常");
        }

        /// <summary>
        /// 得到目标文章ID的评论计数
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public int GetCommentCount(int PostID)
        {
            /* 按时间从早到晚排序 */
            string SQL = string.Format("SELECT COUNT(*) FROM {0}.`{1}` WHERE PostID = ?PostID", DataBase, CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                return Convert.ToInt32(MySqlManager.GetRow(MySqlCommand)[0]);
            }
        }

        /// <summary>
        /// 得到目标文章ID下的评论列表
        /// </summary>
        /// <param name="PostID">目标文章ID</param>
        /// <returns></returns>
        public List<Comment> GetCommentList(int PostID)
        {
            List<Comment> CommentList = new List<Comment>();

            /* 按时间从早到晚排序 */
            string SQL = string.Format("SELECT * FROM {0}.`{1}` WHERE PostID = ?PostID ORDER BY Time", DataBase, CommentTable);

            List<MySqlParm> ParmList = new List<MySqlParm>
                {
                    new MySqlParm() { Name = "?PostID", Val = PostID }
                };

            using (MySqlCommand MySqlCommand = MySqlManager.ParmQueryCMD(SQL, ParmList))
            {
                DataTable result = MySqlManager.GetTable(MySqlCommand);

                /* 楼层暂存变量 */
                ushort f = 0;

                foreach (DataRow Row in result.Rows)
                {
                    CommentList.Add(new Comment
                    {
                        /* 数据库中的量 */
                        CommentID = Convert.ToInt32(Row["CommentID"]),
                        PostID = Convert.ToInt32(Row["PostID"]),
                        Name = Convert.ToString(Row["Name"]),
                        Email = Convert.ToString(Row["Email"]),
                        Content = Convert.ToString(Row["Content"]),
                        WebSIte = Convert.ToString(Row["WebSite"]),
                        HEAD = Convert.ToInt32(Row["HEAD"].ToString() == "" ? -1 : Row["HEAD"]),/* HEAD为空值时赋值为-1 */
                        Time = Convert.ToDateTime(Row["Time"]),

                        /* 计算后得到的量 */
                        Floor = ++f
                    });
                }
                return CommentList;
            }
        }
    }
}
