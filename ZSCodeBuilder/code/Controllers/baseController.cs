﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Model;
using Comp;
namespace cnooc.property.manage.Controllers
{
	/// <summary>
	/// 基础数据
	/// </summary>
	public  class baseController:Controller
	{
		D_base dbase = new D_base();
		/// <summary>
		/// 基础数据 列表
		/// </summary>
		public ActionResult baseList(tb_base model)
		{
			int count = 0;
			ViewBag.baseList = dbase.GetList(model, ref count);
			ViewBag.page = Utils.ShowPage(count, model.PageSize, model.PageIndex, 5);
			return View();
		}

		/// <summary>
		/// 基础数据 保存
		/// </summary>
		public JsonResult baseSave(tb_base model)
		{
			if (model == null)
			{
				return ResultTool.jsonResult(false, "参数错误！");
			}
			if(!String.IsNullOrEmpty(model.id))
			{
				bool boolResult = dbase.Update(model);
				return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "更新失败！");
			}
			else
			{
				model.id = Guid.NewGuid().ToString("N");
				bool boolResult = dbase.Add(model);
				return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "添加失败！");
			}
		}

		/// <summary>
		/// 基础数据 删除
		/// </summary>
		public JsonResult baseDelete(tb_base model)
		{
			bool boolResult = dbase.Delete(model);
			return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "删除失败！");
		}

		/// <summary>
		/// 基础数据 详情
		/// </summary>
		public ActionResult baseInfo(tb_base model)
		{
			model = dbase.GetInfo(model);
			return View(model??new tb_base());
		}

	}
}

