﻿using Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ZSCodeBuilder.Template
{
    public class Template:IDisposable
    {
        public Template(string sPath)
        {
            Load(sPath);
        }
        public string KeyWord;
        /// <summary>
        /// 获取或设置模版HTML代码
        /// </summary>
        public string HTML { get { return _html.ToString(); } set { _html = new StringBuilder(value); } }
        private StringBuilder _html;
        /// <summary>
        /// 读取文件流
        /// </summary>
        /// <param name="sPath"></param>
        private void Load(string sPath)
        {
            string tempContent = "";
            string filePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TemplateDir"] + sPath);
            try
            {
                //打开文件
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));
                //读取流
                tempContent = reader.ReadToEnd();
                reader.Dispose();
                //  Utils.Cache(Config.TemplateDir + sPath, tempContent, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddDays(2), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Template.Err:" + sPath);
                HttpContext.Current.Response.Write("<br />Message:" + ex.Message);
                HttpContext.Current.Response.End();
            }
            _html = new StringBuilder(tempContent + "<!-- Read -->");

            //关闭流
        }
        /// <summary>
        /// 模板替换
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void Replace(string oldValue, string newValue)
        {
            _html.Replace(oldValue, newValue);
        }
        /// <summary>
        /// 获取模版中某部分模版
        /// </summary>
        /// <param name="BStr">开始标记</param>
        /// <param name="EStr">结束标记</param>
        /// <returns>获取的模版</returns>
        public string GetPartContent(string BStr, string EStr)
        {
            string PartStr = string.Empty;
            if (_html.ToString().IndexOf(BStr) > 0)
            {
                PartStr = _html.ToString().Substring(_html.ToString().IndexOf(BStr) + BStr.Length);
                PartStr = PartStr.Substring(0, PartStr.IndexOf(EStr));

            }
            return PartStr;
        }
        /// <summary>
        /// 获取已经处理过的LoopContent标签内容
        /// </summary>
        /// <param name="dt">数据源(DataTable)</param>
        /// <param name="LoopTempate">模版内容</param>
        /// <returns></returns>
        public string GetLoopContent(DataTable dt, string LoopTempate)
        {
            if (dt == null)
            { return "当前无匹配记录！"; }
            LoopTempate = LoopTempate.Replace("{$", "{");
            StringBuilder LoopContent = new StringBuilder();
            foreach (DataRow dRow in dt.Rows)
            {
                Regex Re = new Regex(@"{(.[^{}]+)}");
                MatchCollection Mats = Re.Matches(LoopTempate);
                StringBuilder LoopItem = new StringBuilder(LoopTempate);
                foreach (Match Mat in Mats)
                {
                    LoopItem.Replace(Mat.Value, GetRowsHTML(dRow, Re.Replace(Mat.Value, "$1")));
                }
                LoopContent.Append(LoopItem.ToString());
                LoopItem = null; Re = null; Mats = null;
            }
            //LoopContent.Append("\n<!--<TemplateBegin>" + LoopTempate + "<TemplateEnd>-->\n");
            return LoopContent.ToString();
        }
        private string GetRowsHTML(DataRow dRow, string ParaStr)
        {
            ParaStr = ParaStr.Replace("(", ",");
            ParaStr = ParaStr.Replace(")", "");
            string[] Paras = ParaStr.Split(',');
            string labelName = Paras[0].ToLower();
            string DataStr = string.Empty;
            switch (labelName)
            {
                case "id":
                    return dRow["Id"].ToString();
                case "name":
                    return dRow["Name"].ToString();
                case "details":
                    return dRow["Details"].ToString();
                case "softtype":
                    //DBUtil db = new DBUtil();

                    //return db.GetSoftTypeStr(Convert.ToInt32(dRow["SoftType"]));


                default:
                    return dRow[labelName].ToString();// "默认的";
            }
        }

        /// <summary>
        /// 获取已经处理过的LoopContent标签内容
        /// </summary>
        /// <param name="dt">数据源(DataTable)</param>
        /// <param name="LoopTempate">模版内容</param>
        /// <returns></returns>
        //public string GetLoopContent(List<E_Soft> list, string LoopTempate)
        //{
        //    if (list == null)
        //    {
        //        return "";
        //    }
        //    if (list.Count < 1)
        //    { return ""; }
        //    LoopTempate = LoopTempate.Replace("{$", "{");
        //    StringBuilder LoopContent = new StringBuilder();
        //    for (int i = 0; i < list.Count; i++)
        //    {

        //        Regex Re = new Regex(@"{(.[^{}]+)}");
        //        MatchCollection Mats = Re.Matches(LoopTempate);
        //        StringBuilder LoopItem = new StringBuilder(LoopTempate);
        //        foreach (Match Mat in Mats)
        //        {
        //            LoopItem.Replace(Mat.Value, GetRowsHTML(list[i], Re.Replace(Mat.Value, "$1")));
        //        }
        //        LoopContent.Append(LoopItem.ToString());
        //        LoopItem = null; Re = null; Mats = null;
        //    }

        //    return LoopContent.ToString();
        //}
        //private string GetRowsHTML(E_Soft eSoft, string ParaStr)
        //{
        //    ParaStr = ParaStr.Replace("(", ",");
        //    ParaStr = ParaStr.Replace(")", "");
        //    string[] Paras = ParaStr.Split(',');
        //    string labelName = Paras[0].ToLower();
        //    string DataStr = string.Empty;
        //    switch (labelName)
        //    {
        //        case "id":
        //            return eSoft.ID.ToString();
        //        case "name":
        //            if (string.IsNullOrEmpty(KeyWord))
        //            {
        //                return eSoft.Name;
        //            }
        //            else
        //            {

        //                return eSoft.Name.Replace(KeyWord, "<font style=\"font-style:normal;color:#cc0000;\"><b>" + KeyWord + "</b></font>");
        //            }
        //        case "details":
        //            return eSoft.Details;
        //        case "softtype":
        //            if (eSoft.SoftType == 2)
        //            {
        //                return "电影";
        //            }
        //            else if (eSoft.SoftType == 1)
        //            {
        //                return "图片";
        //            }
        //            else
        //            {
        //                return "未知";
        //            }
        //        case "hash":
        //            return eSoft.Hash;
        //        case "length":
        //            return eSoft.Length;
        //        case "filecount":
        //            return eSoft.FileCount.ToString();
        //        case "hit":
        //            return eSoft.Hit.ToString();
        //        case "updatetime":
        //            return eSoft.UpdateTime.ToString("yyyy-MM-dd");
        //        case "url":
        //            return eSoft.URL;
        //        default:
        //            return labelName;// "默认的";
        //    }
        //}



        #region 【Cl_Loop】标签处理
        /// <summary>
        /// 处理【Cl_Loop】【/Cl_Loop】超级标签
        /// </summary>
        public void ReplaceSuperLoop()
        {
            Regex Re = new Regex(@"【Cl_Loop\((.[^\)]*)\)】(.[^\【]*)【\/Cl_Loop】");
            MatchCollection Mats = Re.Matches(_html.ToString());
            foreach (Match Mat in Mats)
            {
                //定义变量
                string sModule = "article";
                int iNotTopNumber = 0;
                int iTopNumber = 5;
                int iChannelID = 0;
                int iClassID = 0;
                int iSpecialID = 0;
                int isHot = 0;
                int isElite = 0;
                string arrClassID = string.Empty;
                string sWhere = string.Empty;
                string sOrder = string.Empty;
                string Rows = "5";
                int Cols = 1;
                string ColTemplate = string.Empty;
                string sUserName = string.Empty;
                string LoopTemplate = Re.Replace(Mat.Value, "$2").Trim();
                string[] TempValue = Re.Replace(Mat.Value, "$1").Trim().Split(';');
                for (int i = 0; i < TempValue.Length; i++)
                {
                    string[] Param = TempValue[i].Split(':');
                    switch (Param[0].ToLower())
                    {
                        case "module": sModule = Param[1].ToLower(); break;
                        //  case "topnum": iTopNumber = Utils.GetInt(Param[1]); break;
                        //  case "channelid": iChannelID = GetEval(Param[1]); break;
                        //  case "classid": iClassID = GetEval(Param[1]); break;
                        case "arrclassid": arrClassID = Param[1].Replace(" ", ""); break;

                        //  case "ishot": isHot = Utils.GetInt(Param[1]); break;
                        //  case "iselite": isElite = Utils.GetInt(Param[1]); break;
                        case "where": sWhere = Param[1]; break;
                        case "order": sOrder = Param[1].ToString() + ","; break;
                        case "rows": Rows = Param[1]; break;
                        //   case "cols": Cols = Utils.GetInt(Param[1]); break;
                        case "coltemplate": ColTemplate = Param[1]; break;
                        case "name": sUserName = Param[1].ToString(); break;
                    }
                }

                #region 查询构造
                StringBuilder WhereStr = new StringBuilder();
                if (arrClassID != string.Empty)
                {
                    WhereStr.Append(" Where ClassID in (" + arrClassID + ")");
                }
                else if (iClassID > 0)
                {
                    arrClassID = iClassID.ToString();
                    if (arrClassID.IndexOf(",") > 0)
                    {
                        WhereStr.Append(" Where ClassID in (" + arrClassID + ")");
                    }
                    else
                    {
                        WhereStr.Append(" Where ClassID = " + iClassID.ToString() + "");
                    }
                }
                else if (iChannelID > 0)
                {
                    WhereStr.Append(" Where ChannelID = " + iChannelID.ToString() + "");
                }
                if (iSpecialID > 0)
                {
                    if (WhereStr.ToString() == "")
                        WhereStr.Append(" Where SpecialID = " + iSpecialID.ToString() + "");
                    else
                        WhereStr.Append(" and SpecialID = " + iSpecialID.ToString() + "");
                }
                if (isHot == 1)
                {
                    if (WhereStr.ToString() == "")
                        WhereStr.Append(" Where Hits>=150");
                    else
                        WhereStr.Append(" and Hits>=150");
                }
                if (isElite == 1)
                {
                    if (WhereStr.ToString() == "")
                        WhereStr.Append(" Where Elite=1");
                    else
                        WhereStr.Append(" and Elite=1");
                }
                if (isHot != 1 && isElite != 1)
                {
                    if (WhereStr.ToString() == "")
                        WhereStr.Append(" Where Passed=1");
                    else
                        WhereStr.Append(" and Passed=1");
                }
                if (sWhere != "")
                {
                    if (WhereStr.ToString() == "")
                        WhereStr.Append(" Where " + sWhere + " ");
                    else
                        WhereStr.Append(" and " + sWhere + " ");
                }
                if (iTopNumber < 1 || iTopNumber > 20) { iTopNumber = 20; }

                #endregion
                string cacheStr = WhereStr.Length < 1 ? "where" : WhereStr.ToString().Replace(" ", "");
                // string cacheKey = Utils.Md5("ReplaceSuperLoop" + sModule + cacheStr + iTopNumber);
                //==================================================================================
                //  DataTable oTable = (DataTable)MemCache.Get(cacheKey);
                // if (oTable == null || _cacheTime == 0)
                //DataTable oTable=new DataTable();
                //  if(true)
                //  {
                string sql = "";
                switch (sModule)
                {
                    case "soft":
                        //  sql = string.Format("SELECT TOP 20 {0} FROM [Cl_Soft] {1} ORDER BY {2}SoftID DESC", SelectSQL.SList, WhereStr.ToString(), sOrder);
                        break;

                    default:
                        //sql = string.Format("SELECT TOP 20 {0} FROM [Cl_Article] {1} ORDER BY {2}ArticleID DESC", SelectSQL.AList, WhereStr.ToString(), sOrder);
                        break;
                }
                //   DataTable oTable = XKCmd.ExecuteTable();
                //   MemCache.Set(cacheKey, oTable, DateTime.Now.AddMinutes(20));
                //  _html.Replace(Mat.Value, "<!-- Loop Query -->" + ReplaceSuperLoopInfoList(sModule, iTopNumber, Rows, Cols, LoopTemplate, ColTemplate, oTable));
                //     }
                //   else
                //  {
                //    _html.Replace(Mat.Value, "<!-- Loop Cache -->" + ReplaceSuperLoopInfoList(sModule, iTopNumber, Rows, Cols, LoopTemplate, ColTemplate, oTable));
                //  }
                //  oTable.Dispose();
            }
        }
        /// <summary>
        /// 处理【Cl_Loop】【/Cl_Loop】超级标签（模版交替部分）
        /// </summary>
        /// <param name="sModule">模块名称</param>
        /// <param name="Rows">换行参数</param>
        /// <param name="Cols">换列参数</param>
        /// <param name="LoopTemplate">循环模版</param>
        /// <param name="ColTemplate">换列模版</param>
        /// <param name="Dt">DataTable数据对像</param>
        /// <returns></returns>
        public string ReplaceSuperLoopInfoList(string sModule, int topNumber, string Rows, int Cols, string LoopTemplate, string ColTemplate, DataTable Dt)
        {
            LoopTemplate = LoopTemplate.Replace("{$", "{");
            string[] ArrTemplate = LoopTemplate.Split(new string[] { "||" }, StringSplitOptions.None);
            string[] ArrRow = Rows.Split('|');
            int TNums = ArrTemplate.Length;
            int TRows = 0, iCount = 0;
            int i = 0, j = 0, n = 0;
            StringBuilder ReturnHTML = new StringBuilder();
            StringBuilder sTemp; Regex Re; MatchCollection Mats;
            foreach (DataRow Row in Dt.Rows)
            {
                if ((TNums > 1) && (j > 0))
                {
                    TRows = Utils.GetInt(ArrRow[n]);
                    if (j >= TRows)
                    {
                        j = 0;
                        n++;
                        if (n >= TNums) n = 0;
                    }
                    sTemp = new StringBuilder(ArrTemplate[n]);
                }
                else
                {
                    sTemp = new StringBuilder(ArrTemplate[n]);
                }
                i++; j++;
                Re = new Regex(@"{(.[^{}]+)}");
                Mats = Re.Matches(sTemp.ToString());
                foreach (Match Mat in Mats)
                {
                    sTemp.Replace(Mat.Value, GetRowsHTML(Row, Re.Replace(Mat.Value, "$1")));
                }
                ReturnHTML.Append(sTemp.ToString());
                sTemp = null; Mats = null;
                if ((i % Cols) == 0) ReturnHTML.Append(ColTemplate);
                iCount++;
                if (iCount >= topNumber) break;
            }
            return ReturnHTML.ToString();
        }

        #endregion
        /// <summary>
        /// 替换所有标签
        /// </summary>
        public void ReplaceAllFlag()
        {
            //_html.Replace("{$showheader}", Utils.ReadTemplate("_Header.htm"));
            //_html.Replace("{$showfooter}", Utils.ReadTemplate("_Footer.htm"));
        }


        public void Dispose()
        {
            _html = null;
        }
    }
}
