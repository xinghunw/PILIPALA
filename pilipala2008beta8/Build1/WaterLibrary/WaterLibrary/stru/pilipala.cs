﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WaterLibrary.com.basic;
using WaterLibrary.com.MySQL;
using WaterLibrary.stru.MySQL;
using WaterLibrary.stru.pilipala.DB;
using WaterLibrary.stru.pilipala.PostKey;

namespace WaterLibrary.stru.pilipala
{
    namespace DB
    {
        /// <summary>
        /// 啪啦数据库接口
        /// </summary>
        interface IPLDataBase
        {
            /// <summary>
            /// 文本表
            /// </summary>
            PLTables Tables { get; }
            /// <summary>
            /// 文本视图
            /// </summary>
            PLViews Views { get; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            MySqlManager MySqlManager { get; }
        }

        /// <summary>
        /// 索引表数据接口
        /// </summary>
        public interface ITableIndex
        {

            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            DateTime CT { get; set; }
            /// <summary>
            /// 模式
            /// </summary>
            string Mode { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            string Type { get; set; }
            /// <summary>
            /// 隶属用户
            /// </summary>
            string User { get; set; }
            /// <summary>
            /// 浏览计数
            /// </summary>
            int UVCount { get; set; }
            /// <summary>
            /// 星星计数
            /// </summary>
            int StarCount { get; set; }

        }
        /// <summary>
        /// 备份表数据接口
        /// </summary>
        public interface ITableBackup
        {
            /// <summary>
            /// 索引
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            DateTime LCT { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            string Title { get; set; }
            /// <summary>
            /// 摘要
            /// </summary>
            string Summary { get; set; }
            /// <summary>
            /// 正文
            /// </summary>
            string Content { get; set; }
            /// <summary>
            /// 归档
            /// </summary>
            string Archiv { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            string Label { get; set; }
            /// <summary>
            /// 封面
            /// </summary>
            string Cover { get; set; }
        }
        /// <summary>
        /// 用户表数据接口
        /// </summary>
        public interface ITableUser
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            int ID { get; set; }
            /// <summary>
            /// GUID标识
            /// </summary>
            string GUID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            string Name { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            string Note { get; set; }
        }

        /// <summary>
        /// 数据库表集合
        /// </summary>
        public struct PLTables
        {
            /// <summary>
            /// 初始化表名结构
            /// </summary>
            /// <parmm name="root"></parmm>
            /// <parmm name="IndexTable"></parmm>
            /// <parmm name="BackupTable"></parmm>
            /// <parmm name="text_sub"></parmm>
            public PLTables(string User, string Index, string Backup) : this()
            {
                this.User = User;
                this.Index = Index;
                this.Backup = Backup;
            }

            /// <summary>
            /// 用户表
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// 索引表
            /// </summary>
            public string Index { get; set; }
            /// <summary>
            /// 主表
            /// </summary>
            public string Backup { get; set; }
        }
        /// <summary>
        /// 数据库视图视图集合
        /// </summary>
        public struct PLViews
        {
            /// <summary>
            /// 初始化视图名结构
            /// </summary>
            /// <param name="Index">索引视图</param>
            /// <param name="Backup">备份视图</param>
            /// <param name="Union">总视图</param>
            public PLViews(string Index, string Backup, string Union) : this()
            {
                this.Index = Index;
                this.Backup = Backup;
                this.Union = Union;
            }
            /// <summary>
            /// 索引视图
            /// </summary>
            public string Index { get; set; }
            /// <summary>
            /// 备份视图
            /// </summary>
            public string Backup { get; set; }
            /// <summary>
            /// 联合视图(索引表JOIN主表)
            /// </summary>
            public string Union { get; set; }
        }

        /// <summary>
        /// 啪啦数据库信息
        /// </summary>
        public struct PLDB : IPLDataBase
        {
            /// <summary>
            /// 数据表
            /// </summary>
            public PLTables Tables { get; set; }
            /// <summary>
            /// 数据视图
            /// </summary>
            public PLViews Views { get; set; }
            /// <summary>
            /// 数据库管理器实例
            /// </summary>
            public MySqlManager MySqlManager { get; set; }
        }
    }

    /// <summary>
    /// 啪啦数据读取器接口
    /// </summary>
    interface IPLDataReader
    {
        /// <summary>
        /// 获得全部文本ID列表
        /// </summary>
        /// <returns></returns>
        List<int> GetID();
        /// <summary>
        /// 通用文章匹配器
        /// </summary>
        /// <typeparmm name="T">被匹配的键类型</typeparmm>
        /// <parmm name="Value">被匹配的键值</parmm>
        /// <returns></returns>
        List<int> MatchID<T>(string Value) where T : PostKey.IPostKey;

        /// <summary>
        /// 取得指定文本 ID 的下一个文本 ID（按照ID升序查找）
        /// </summary>
        /// <parmm name="current_ID">当前文本序列号</parmm>
        /// <returns>错误返回 -1</returns>
        int NextID(int ID);
        /// <summary>
        /// 取得指定文本 ID 的上一个文本 ID（按照ID升序查找）
        /// </summary>
        /// <parmm name="current_ID">当前文本序列号</parmm>
        /// <returns>错误返回 -1</returns>
        int PrevID(int ID);
    }
    /// <summary>
    /// 啪啦数据修改器接口
    /// </summary>
    interface IPLDataUpdater
    {
        /// <summary>
        /// 将目标文章状态标记为展示
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool UnsetMode(int ID);
        /// <summary>
        /// 将目标文章状态标记为隐藏
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool HiddenMode(int ID);
        /// <summary>
        /// 将目标文章状态标记为计划中
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool ScheduledMode(int ID);
        /// <summary>
        /// 将目标文章状态标记为已归档
        /// </summary>
        /// <param name="ID">目标文章ID</param>
        /// <returns></returns>
        bool ArchivedMode(int ID);

        /// <summary>
        /// 通用文章属性更新器
        /// </summary>
        /// <typeparam name="T">文章属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Value">新属性值</param>
        /// <returns></returns>
        bool UpdateIndex<T>(int ID, object Value) where T : IPostKey;
        /// <summary>
        /// 通用文章属性更新器
        /// </summary>
        /// <typeparam name="T">文章属性类型</typeparam>
        /// <param name="ID">目标文章ID</param>
        /// <param name="Value">新属性值</param>
        /// <returns></returns>
        bool UpdateBackup<T>(int ID, object Value) where T : IPostKey;
    }
    /// <summary>
    /// 啪啦数据计数器接口
    /// </summary>
    interface IPLDataCounter
    {
        /// <summary>
        /// 文章计数
        /// </summary>
        int PostCount { get; }
        /// <summary>
        /// 拷贝计数
        /// </summary>
        int BackupCount { get; }
    }

    /// <summary>
    /// 用户结构
    /// </summary>
    public class User : ITableUser
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="Key">索引名</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get
            {
                /* 通过反射获取属性 */
                return GetType().GetProperty(Key).GetValue(this);
            }
            set
            {
                /* 通过反射设置属性 */
                System.Type ThisType = GetType();
                System.Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
                ThisType.GetProperty(Key).SetValue(this, Convert.ChangeType(value, KeyType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }
    }

    namespace PostKey
    {
        /// <summary>
        /// 表键值接口
        /// </summary>
        public interface IPostKey
        {

        }

        /// <summary>
        /// 文章索引
        /// </summary>
        public struct ID : IPostKey
        {

        }
        /// <summary>
        /// 文章全局标识
        /// </summary>
        public struct GUID : IPostKey
        {

        }

        /// <summary>
        /// 文章标题
        /// </summary>
        public struct Title : IPostKey
        {

        }
        /// <summary>
        /// 文章摘要
        /// </summary>
        public struct Summary : IPostKey
        {

        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public struct Content : IPostKey
        {

        }
        /// <summary>
        /// 文章标题
        /// </summary>
        public struct Cover : IPostKey
        {

        }

        /// <summary>
        /// 文章归档
        /// </summary>
        public struct Archiv : IPostKey
        {

        }
        /// <summary>
        /// 文章标签
        /// </summary>
        public struct Label : IPostKey
        {

        }

        /// <summary>
        /// 文章模式
        /// </summary>
        public struct Mode : IPostKey
        {

        }
        /// <summary>
        /// 文章类型
        /// </summary>
        public struct Type : IPostKey
        {

        }
        /// <summary>
        /// 归属用户
        /// </summary>
        public struct User : IPostKey
        {

        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public struct CT : IPostKey
        {

        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public struct LCT : IPostKey
        {

        }

        /// <summary>
        /// 浏览计数
        /// </summary>
        public struct UVCount : IPostKey
        {

        }
        /// <summary>
        /// 星星计数
        /// </summary>
        public struct StarCount : IPostKey
        {

        }
    }
    /// <summary>
    /// 文章结构
    /// </summary>
    public class Post : ITableIndex, ITableBackup
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="Key">索引名</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get
            {
                /* 通过反射获取属性 */
                return GetType().GetProperty(Key).GetValue(this);
            }
            set
            {
                /* 通过反射设置属性 */
                System.Type ThisType = GetType();
                System.Type KeyType = ThisType.GetProperty(Key).GetValue(this).GetType();
                ThisType.GetProperty(Key).SetValue(this, Convert.ChangeType(value, KeyType));
            }
        }
        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> ItemArray()
        {
            yield return ID;
            yield return GUID;

            yield return Title;
            yield return Summary;
            yield return Content;
            yield return Cover;

            yield return Archiv;
            yield return Label;

            yield return Mode;
            yield return Type;
            yield return User;

            yield return CT;
            yield return LCT;

            yield return UVCount;
            yield return StarCount;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public Post()
        {
            /* -1表示未被赋值，同时也于数据库的非负冲突 */
            ID = -1;
            GUID = "";

            Title = "";
            Summary = "";
            Content = "";
            Cover = "";

            Archiv = "";
            Label = "";

            Mode = "";
            Type = "";
            User = "";

            CT = new DateTime();
            LCT = new DateTime();

            UVCount = -1;
            StarCount = -1;
        }

        /// <summary>
        /// 基于Title,Summary和Content的MD5签名(自动生成)
        /// </summary>
        public string MD5
        {
            get { return ConvertH.ToMD5(Title + Summary + Content); }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 全局标识
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Archiv { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 文章模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 归属用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CT { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LCT { get; set; }

        /// <summary>
        /// 访问计数
        /// </summary>
        public int UVCount { get; set; }
        /// <summary>
        /// 星星计数
        /// </summary>
        public int StarCount { get; set; }
    }
}
