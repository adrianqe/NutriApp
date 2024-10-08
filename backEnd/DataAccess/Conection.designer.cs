﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace backEnd.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="NutriDataBase")]
	public partial class ConectionDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ConectionDataContext() : 
				base(global::backEnd.Properties.Settings.Default.NutriDataBaseConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ConectionDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ConectionDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ConectionDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ConectionDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Registrar_Nuevo_Usuario")]
		public int SP_Registrar_Nuevo_Usuario([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Nombre", DbType="NVarChar(100)")] string nombre, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(150)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Password_Hash", DbType="NVarChar(255)")] string password_Hash, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), nombre, email, password_Hash, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(3)));
			mensaje = ((string)(result.GetParameterValue(4)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Registrar_Feedback")]
		public int SP_Registrar_Feedback([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Usuario_ID", DbType="Int")] System.Nullable<int> usuario_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Producto_ID", DbType="Int")] System.Nullable<int> producto_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Calificacion", DbType="Int")] System.Nullable<int> calificacion, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Comentario", DbType="NVarChar(500)")] string comentario, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), usuario_ID, producto_ID, calificacion, comentario, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(4)));
			mensaje = ((string)(result.GetParameterValue(5)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Escanear_Codigo")]
		public int SP_Escanear_Codigo([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Codigo_Barras", DbType="NVarChar(50)")] string codigo_Barras, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), codigo_Barras, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(1)));
			mensaje = ((string)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Eliminar_Usuario")]
		public int SP_Eliminar_Usuario([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Usuario_ID", DbType="Int")] System.Nullable<int> usuario_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), usuario_ID, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(1)));
			mensaje = ((string)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Eliminar_Producto")]
		public int SP_Eliminar_Producto([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Producto_ID", DbType="Int")] System.Nullable<int> producto_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), producto_ID, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(1)));
			mensaje = ((string)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Eliminar_Feedback")]
		public int SP_Eliminar_Feedback([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Feedback_ID", DbType="Int")] System.Nullable<int> feedback_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), feedback_ID, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(1)));
			mensaje = ((string)(result.GetParameterValue(2)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Consultar_Productos_Escaneados")]
		public ISingleResult<SP_Consultar_Productos_EscaneadosResult> SP_Consultar_Productos_Escaneados([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Usuario_ID", DbType="Int")] System.Nullable<int> usuario_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), usuario_ID, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(1)));
			mensaje = ((string)(result.GetParameterValue(2)));
			return ((ISingleResult<SP_Consultar_Productos_EscaneadosResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Actualizar_Usuario")]
		public int SP_Actualizar_Usuario([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Usuario_ID", DbType="Int")] System.Nullable<int> usuario_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Nombre", DbType="NVarChar(100)")] string nombre, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(150)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Password_Hash", DbType="NVarChar(255)")] string password_Hash, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), usuario_ID, nombre, email, password_Hash, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(4)));
			mensaje = ((string)(result.GetParameterValue(5)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SP_Actualizar_Producto")]
		public int SP_Actualizar_Producto([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Producto_ID", DbType="Int")] System.Nullable<int> producto_ID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Nombre", DbType="NVarChar(100)")] string nombre, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Categoria", DbType="NVarChar(50)")] string categoria, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Marca", DbType="NVarChar(50)")] string marca, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Informacion_Nutricional", DbType="NVarChar(MAX)")] string informacion_Nutricional, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Exito", DbType="Bit")] ref System.Nullable<bool> exito, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Mensaje", DbType="NVarChar(255)")] ref string mensaje)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), producto_ID, nombre, categoria, marca, informacion_Nutricional, exito, mensaje);
			exito = ((System.Nullable<bool>)(result.GetParameterValue(5)));
			mensaje = ((string)(result.GetParameterValue(6)));
			return ((int)(result.ReturnValue));
		}
	}
	
	public partial class SP_Consultar_Productos_EscaneadosResult
	{
		
		private string _Nombre;
		
		private System.DateTime _Fecha_Escaneo;
		
		public SP_Consultar_Productos_EscaneadosResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Nombre", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Nombre
		{
			get
			{
				return this._Nombre;
			}
			set
			{
				if ((this._Nombre != value))
				{
					this._Nombre = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Fecha_Escaneo", DbType="DateTime2 NOT NULL")]
		public System.DateTime Fecha_Escaneo
		{
			get
			{
				return this._Fecha_Escaneo;
			}
			set
			{
				if ((this._Fecha_Escaneo != value))
				{
					this._Fecha_Escaneo = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
