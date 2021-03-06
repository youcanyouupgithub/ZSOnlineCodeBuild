﻿using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Model;
using System.Linq;
using System.Collections.Generic;
using Dapper;
namespace DAL
{
	/// <summary>
	/// 数据访问类:D_room
	/// </summary>
	public partial class D_room
	{
		public D_room()
		{}
		#region  Method

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(tb_room model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from tb_room");
			strSql.Append("  where id=@id ");
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				int count = conn.Execute(strSql.ToString(), model);
				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(tb_room model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_room(");
			strSql.Append("id,buildingid,floor,roomnumber,roomtype,hold,housetype,direction,space,fee,status,isopen,isdel,sortnum,addtime)");
			strSql.Append(" values (");
			strSql.Append("@id,@buildingid,@floor,@roomnumber,@roomtype,@hold,@housetype,@direction,@space,@fee,@status,@isopen,@isdel,@sortnum,@addtime)");
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				int count = conn.Execute(strSql.ToString(), model);
				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(tb_room model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder setSql=new StringBuilder();
			strSql.Append("update tb_room set ");
			if(!String.IsNullOrEmpty(model.buildingid))
			{
				setSql.Append( "buildingid=@buildingid,");
			}
			if(!String.IsNullOrEmpty(model.floor))
			{
				setSql.Append( "floor=@floor,");
			}
			if(!String.IsNullOrEmpty(model.roomnumber))
			{
				setSql.Append( "roomnumber=@roomnumber,");
			}
			if(model.roomtype!=null)
			{
				setSql.Append( "roomtype=@roomtype,");
			}
			if(!String.IsNullOrEmpty(model.hold))
			{
				setSql.Append( "hold=@hold,");
			}
			if(!String.IsNullOrEmpty(model.housetype))
			{
				setSql.Append( "housetype=@housetype,");
			}
			if(!String.IsNullOrEmpty(model.direction))
			{
				setSql.Append( "direction=@direction,");
			}
			if(!String.IsNullOrEmpty(model.space))
			{
				setSql.Append( "space=@space,");
			}
			if(!String.IsNullOrEmpty(model.fee))
			{
				setSql.Append( "fee=@fee,");
			}
			if(model.status!=null)
			{
				setSql.Append( "status=@status,");
			}
			if(model.isopen!=null)
			{
				setSql.Append( "isopen=@isopen,");
			}
			if(model.isdel)
			{
				setSql.Append( "isdel=@isdel,");
			}
			if(model.sortnum!=null)
			{
				setSql.Append( "sortnum=@sortnum,");
			}
			if(model.addtime!=null)
			{
				setSql.Append( "addtime=@addtime,");
			}
			strSql.Append(setSql.ToString().TrimEnd(','));
			strSql.Append(" where id=@id ");
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				int count = conn.Execute(strSql.ToString(), model);
				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(tb_room model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_room ");
			strSql.Append(" where id=@id " );
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				int count = conn.Execute(strSql.ToString(), model);
				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_room ");
			strSql.Append(" where id in ("+idlist + ")  ");
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				int count = conn.Execute(strSql.ToString());
				if (count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<tb_room> GetList(tb_room model, ref int total)
		{
			List<tb_room> list;
			StringBuilder strSql=new StringBuilder();
			StringBuilder whereSql = new StringBuilder(" where 1 = 1 ");
			strSql.Append(" select  ROW_NUMBER() OVER(ORDER BY id desc) AS RID, * from tb_room ");
			if(!String.IsNullOrEmpty(model.buildingid))
			{
				whereSql.Append( " and buildingid=@buildingid");
			}
			if(!String.IsNullOrEmpty(model.floor))
			{
				whereSql.Append( " and floor=@floor");
			}
			if(!String.IsNullOrEmpty(model.roomnumber))
			{
				whereSql.Append( " and roomnumber=@roomnumber");
			}
			if(model.roomtype!=null)
			{
				whereSql.Append( " and roomtype=@roomtype");
			}
			if(!String.IsNullOrEmpty(model.hold))
			{
				whereSql.Append( " and hold=@hold");
			}
			if(!String.IsNullOrEmpty(model.housetype))
			{
				whereSql.Append( " and housetype=@housetype");
			}
			if(!String.IsNullOrEmpty(model.direction))
			{
				whereSql.Append( " and direction=@direction");
			}
			if(!String.IsNullOrEmpty(model.space))
			{
				whereSql.Append( " and space=@space");
			}
			if(!String.IsNullOrEmpty(model.fee))
			{
				whereSql.Append( " and fee=@fee");
			}
			if(model.status!=null)
			{
				whereSql.Append( " and status=@status");
			}
			if(model.isopen!=null)
			{
				whereSql.Append( " and isopen=@isopen");
			}
			if(model.isdel)
			{
				whereSql.Append( " and isdel=@isdel");
			}
			if(model.sortnum!=null)
			{
				whereSql.Append( " and sortnum=@sortnum");
			}
			if(model.addtime!=null)
			{
				whereSql.Append( " and addtime=@addtime");
			}
			strSql.Append(whereSql);
			string CountSql = "SELECT COUNT(1) as RowsCount FROM (" + strSql.ToString() + ") AS CountList";
			string pageSqlStr = "select * from ( " + strSql.ToString() + " ) as Temp_PageData where Temp_PageData.RID BETWEEN {0} AND {1}";
			pageSqlStr = string.Format(pageSqlStr, (model.PageSize * (model.PageIndex - 1) + 1).ToString(), (model.PageSize * model.PageIndex).ToString());
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				list = conn.Query <tb_room>(pageSqlStr, model)?.ToList();
				total = conn.ExecuteScalar<int>(CountSql, model);
			}
			return list;
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public tb_room GetInfo(tb_room model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select * from tb_room");
			strSql.Append("  where id=@id ");
			using (IDbConnection conn = DapperHelper.OpenConnection())
			{
				model = conn.Query <tb_room>(strSql.ToString(), model)?.FirstOrDefault();
			}
			return model;
		}
		#endregion  Method
	}
}

