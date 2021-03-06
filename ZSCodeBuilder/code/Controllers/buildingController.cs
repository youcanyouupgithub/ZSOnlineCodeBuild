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
	/// 楼宇
	/// </summary>
	public  class buildingController:Controller
	{
		D_building dbuilding = new D_building();
		/// <summary>
		/// 楼宇 列表
		/// </summary>
		public ActionResult buildingList(tb_building model)
		{
			int count = 0;
			ViewBag.buildingList = dbuilding.GetList(model, ref count);
			ViewBag.page = Utils.ShowPage(count, model.PageSize, model.PageIndex, 5);
			return View();
		}

		/// <summary>
		/// 楼宇 保存
		/// </summary>
		public JsonResult buildingSave(tb_building model)
		{
			if (model == null)
			{
				return ResultTool.jsonResult(false, "参数错误！");
			}
			if(!String.IsNullOrEmpty(model.id))
			{
				bool boolResult = dbuilding.Update(model);
				return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "更新失败！");
			}
			else
			{
				model.id = Guid.NewGuid().ToString("N");
				bool boolResult = dbuilding.Add(model);
				return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "添加失败！");
			}
		}

		/// <summary>
		/// 楼宇 删除
		/// </summary>
		public JsonResult buildingDelete(tb_building model)
		{
			bool boolResult = dbuilding.Delete(model);
			return ResultTool.jsonResult(boolResult, boolResult ? "成功！" : "删除失败！");
		}

		/// <summary>
		/// 楼宇 详情
		/// </summary>
		public ActionResult buildingInfo(tb_building model)
		{
			model = dbuilding.GetInfo(model);
			return View(model??new tb_building());
		}

	}
}

