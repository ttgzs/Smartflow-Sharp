$(function(){
	//页面加载完成之后执行
	pageInit();
});
function pageInit(){
	//创建jqGrid组件
    jQuery("#userlist").jqGrid(
			{
			    url: 'GetUserList',//组件创建完成之后请求数据的url
				datatype : "json",//请求数据返回的类型。可选json,xml,txt
				colNames : [  '姓名', '部门' ],//jqGrid的列显示名字
				colModel : [ //jqGrid每一列的配置信息。包括名字，索引，宽度,对齐方式.....
				             { name: 'EmployeeName', index: '姓名',align:'center', width: 90, sortable: false },
				             { name: 'OrgName', index: '部门', sortable: false }
				           ],
				rowNum : 5,//一页显示多少条
				rowList : [ 5, 10, 15 ],//可供用户选择一页显示多少条
				pager : '#pager',//表格页脚的占位符(一般是div)的id
				/*sortname : 'id',//初始化的时候排序的字段
				sortorder : "desc",//排序方式,可选desc,asc*/
				mtype : "post",//向后台请求数据的ajax的类型。可选post,get
				viewrecords: true,
				pagerpos: 'left',
				recordpos: 'right',
				height: 158,
                width:342,
				shrinkToFit:true,
				rownumbers: true,
				toolbar: [true, 'top']
			});
	/*创建jqGrid的操作按钮容器*/
	/*可以控制界面上增删改查的按钮是否显示*/
	jQuery("#userlist").jqGrid('navGrid', '#pager', { edit: false, search: false, refresh: false, add: false, del: false });
	$("#t_userlist").html(document.getElementById("tbTemplate").innerHTML);

}