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

namespace MyTouristBook
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="adsrush")]
	public partial class blogsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertTable_MyTouristbook_Blog(Table_MyTouristbook_Blog instance);
    partial void UpdateTable_MyTouristbook_Blog(Table_MyTouristbook_Blog instance);
    partial void DeleteTable_MyTouristbook_Blog(Table_MyTouristbook_Blog instance);
    #endregion
		
		public blogsDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["adsrushConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public blogsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public blogsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public blogsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public blogsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Table_MyTouristbook_Blog> Table_MyTouristbook_Blogs
		{
			get
			{
				return this.GetTable<Table_MyTouristbook_Blog>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Table_MyTouristbook_Blogs")]
	public partial class Table_MyTouristbook_Blog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private System.Nullable<int> _blog_id;
		
		private System.Nullable<System.DateTime> _startdate;
		
		private System.Nullable<int> _active;
		
		private System.Nullable<int> _authoraid;
		
		private string _authorusername;
		
		private string _title;
		
		private string _body;
		
		private string _subniche;
		
		private string _imageurl;
		
		private System.Nullable<int> _priority;
		
		private System.Nullable<int> _sponsored;
		
		private System.Nullable<int> _featured;
		
		private System.Nullable<int> _popular;
		
		private string _body2;
		
		private string _body3;
		
		private string _dest1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onblog_idChanging(System.Nullable<int> value);
    partial void Onblog_idChanged();
    partial void OnstartdateChanging(System.Nullable<System.DateTime> value);
    partial void OnstartdateChanged();
    partial void OnactiveChanging(System.Nullable<int> value);
    partial void OnactiveChanged();
    partial void OnauthoraidChanging(System.Nullable<int> value);
    partial void OnauthoraidChanged();
    partial void OnauthorusernameChanging(string value);
    partial void OnauthorusernameChanged();
    partial void OntitleChanging(string value);
    partial void OntitleChanged();
    partial void OnbodyChanging(string value);
    partial void OnbodyChanged();
    partial void OnsubnicheChanging(string value);
    partial void OnsubnicheChanged();
    partial void OnimageurlChanging(string value);
    partial void OnimageurlChanged();
    partial void OnpriorityChanging(System.Nullable<int> value);
    partial void OnpriorityChanged();
    partial void OnsponsoredChanging(System.Nullable<int> value);
    partial void OnsponsoredChanged();
    partial void OnfeaturedChanging(System.Nullable<int> value);
    partial void OnfeaturedChanged();
    partial void OnpopularChanging(System.Nullable<int> value);
    partial void OnpopularChanged();
    partial void Onbody2Changing(string value);
    partial void Onbody2Changed();
    partial void Onbody3Changing(string value);
    partial void Onbody3Changed();
    partial void Ondest1Changing(string value);
    partial void Ondest1Changed();
    #endregion
		
		public Table_MyTouristbook_Blog()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_blog_id", DbType="Int")]
		public System.Nullable<int> blog_id
		{
			get
			{
				return this._blog_id;
			}
			set
			{
				if ((this._blog_id != value))
				{
					this.Onblog_idChanging(value);
					this.SendPropertyChanging();
					this._blog_id = value;
					this.SendPropertyChanged("blog_id");
					this.Onblog_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_startdate", DbType="DateTime")]
		public System.Nullable<System.DateTime> startdate
		{
			get
			{
				return this._startdate;
			}
			set
			{
				if ((this._startdate != value))
				{
					this.OnstartdateChanging(value);
					this.SendPropertyChanging();
					this._startdate = value;
					this.SendPropertyChanged("startdate");
					this.OnstartdateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_active", DbType="Int")]
		public System.Nullable<int> active
		{
			get
			{
				return this._active;
			}
			set
			{
				if ((this._active != value))
				{
					this.OnactiveChanging(value);
					this.SendPropertyChanging();
					this._active = value;
					this.SendPropertyChanged("active");
					this.OnactiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_authoraid", DbType="Int")]
		public System.Nullable<int> authoraid
		{
			get
			{
				return this._authoraid;
			}
			set
			{
				if ((this._authoraid != value))
				{
					this.OnauthoraidChanging(value);
					this.SendPropertyChanging();
					this._authoraid = value;
					this.SendPropertyChanged("authoraid");
					this.OnauthoraidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_authorusername", DbType="NVarChar(500)")]
		public string authorusername
		{
			get
			{
				return this._authorusername;
			}
			set
			{
				if ((this._authorusername != value))
				{
					this.OnauthorusernameChanging(value);
					this.SendPropertyChanging();
					this._authorusername = value;
					this.SendPropertyChanged("authorusername");
					this.OnauthorusernameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_title", DbType="NVarChar(500)")]
		public string title
		{
			get
			{
				return this._title;
			}
			set
			{
				if ((this._title != value))
				{
					this.OntitleChanging(value);
					this.SendPropertyChanging();
					this._title = value;
					this.SendPropertyChanged("title");
					this.OntitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_body", DbType="NVarChar(4000)")]
		public string body
		{
			get
			{
				return this._body;
			}
			set
			{
				if ((this._body != value))
				{
					this.OnbodyChanging(value);
					this.SendPropertyChanging();
					this._body = value;
					this.SendPropertyChanged("body");
					this.OnbodyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_subniche", DbType="NVarChar(500)")]
		public string subniche
		{
			get
			{
				return this._subniche;
			}
			set
			{
				if ((this._subniche != value))
				{
					this.OnsubnicheChanging(value);
					this.SendPropertyChanging();
					this._subniche = value;
					this.SendPropertyChanged("subniche");
					this.OnsubnicheChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_imageurl", DbType="NVarChar(1500)")]
		public string imageurl
		{
			get
			{
				return this._imageurl;
			}
			set
			{
				if ((this._imageurl != value))
				{
					this.OnimageurlChanging(value);
					this.SendPropertyChanging();
					this._imageurl = value;
					this.SendPropertyChanged("imageurl");
					this.OnimageurlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_priority", DbType="Int")]
		public System.Nullable<int> priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				if ((this._priority != value))
				{
					this.OnpriorityChanging(value);
					this.SendPropertyChanging();
					this._priority = value;
					this.SendPropertyChanged("priority");
					this.OnpriorityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sponsored", DbType="Int")]
		public System.Nullable<int> sponsored
		{
			get
			{
				return this._sponsored;
			}
			set
			{
				if ((this._sponsored != value))
				{
					this.OnsponsoredChanging(value);
					this.SendPropertyChanging();
					this._sponsored = value;
					this.SendPropertyChanged("sponsored");
					this.OnsponsoredChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_featured", DbType="Int")]
		public System.Nullable<int> featured
		{
			get
			{
				return this._featured;
			}
			set
			{
				if ((this._featured != value))
				{
					this.OnfeaturedChanging(value);
					this.SendPropertyChanging();
					this._featured = value;
					this.SendPropertyChanged("featured");
					this.OnfeaturedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_popular", DbType="Int")]
		public System.Nullable<int> popular
		{
			get
			{
				return this._popular;
			}
			set
			{
				if ((this._popular != value))
				{
					this.OnpopularChanging(value);
					this.SendPropertyChanging();
					this._popular = value;
					this.SendPropertyChanged("popular");
					this.OnpopularChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_body2", DbType="NVarChar(4000)")]
		public string body2
		{
			get
			{
				return this._body2;
			}
			set
			{
				if ((this._body2 != value))
				{
					this.Onbody2Changing(value);
					this.SendPropertyChanging();
					this._body2 = value;
					this.SendPropertyChanged("body2");
					this.Onbody2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_body3", DbType="NVarChar(4000)")]
		public string body3
		{
			get
			{
				return this._body3;
			}
			set
			{
				if ((this._body3 != value))
				{
					this.Onbody3Changing(value);
					this.SendPropertyChanging();
					this._body3 = value;
					this.SendPropertyChanged("body3");
					this.Onbody3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dest1", DbType="NVarChar(1500)")]
		public string dest1
		{
			get
			{
				return this._dest1;
			}
			set
			{
				if ((this._dest1 != value))
				{
					this.Ondest1Changing(value);
					this.SendPropertyChanging();
					this._dest1 = value;
					this.SendPropertyChanged("dest1");
					this.Ondest1Changed();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591