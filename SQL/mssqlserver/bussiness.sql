create database bussiness
go
use bussiness
go
if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_apply')
            and   type = 'U')
   drop table dbo.t_apply
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_org')
            and   type = 'U')
   drop table dbo.t_org
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_pending')
            and   type = 'U')
   drop table dbo.t_pending
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_record')
            and   type = 'U')
   drop table dbo.t_record
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_role')
            and   type = 'U')
   drop table dbo.t_role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_user')
            and   type = 'U')
   drop table dbo.t_user
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_umr')
            and   type = 'U')
   drop table dbo.t_umr
go

execute sp_revokedbaccess dbo
go

/*==============================================================*/
/* User: dbo                                                    */
/*==============================================================*/
execute sp_grantdbaccess dbo
go

/*==============================================================*/
/* Table: t_apply                                               */
/*==============================================================*/
create table dbo.t_apply (
   IDENTIFICATION       bigint               identity(1, 1),
   FNAME                varchar(50)          collate Chinese_PRC_CI_AS null,
   DESCRIPTION          varchar(1024)        collate Chinese_PRC_CI_AS null,
   STATUS               int                  null,
   STRUCTUREID          varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   INSERTDATE           datetime             null constraint DF_t_apply_INSERTDATE default getdate(),
   SECRETGRADE          varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_apply primary key (IDENTIFICATION)
         on "PRIMARY"
)
on "PRIMARY"
go



/*==============================================================*/
/* Table: t_org                                                 */
/*==============================================================*/
create table dbo.t_org (
   IDENTIFICATION       bigint               identity(1, 1),
   ORGNAME              varchar(50)          collate Chinese_PRC_CI_AS null,
   ORGCODE              varchar(50)          collate Chinese_PRC_CI_AS null,
   PARENTCODE           varchar(50)          collate Chinese_PRC_CI_AS null,
   DESCRIPTION          varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_org primary key (IDENTIFICATION)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_pending                                             */
/*==============================================================*/
create table dbo.t_pending (
   IDENTIFICATION       bigint               identity(1, 1),
   ACTORID              bigint               null,
   NODEID               varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   APPELLATION          varchar(1024)        collate Chinese_PRC_CI_AS null,
   CREATEDATETIME       datetime             null constraint DF_t_pending_CREATEDATETIME default getdate(),
   ACTION               varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_pending primary key (IDENTIFICATION)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_record                                              */
/*==============================================================*/
create table dbo.t_record (
   IDENTIFICATION       bigint               identity(1, 1),
   NODENAME             varchar(512)         collate Chinese_PRC_CI_AS null,
   MESSAGE              varchar(1024)        collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   INSERTDATE           datetime             null constraint DF_t_record_INSERTDATE default getdate(),
   constraint PK_t_record primary key (IDENTIFICATION)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_role                                                */
/*==============================================================*/
create table dbo.t_role (
   Identification       bigint               identity(1, 1),
   Appellation          varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_T_ROLE primary key (Identification)
         on "PRIMARY"
)
on "PRIMARY"
go
/*==============================================================*/
/* Table: t_umr                                                 */
/*==============================================================*/
create table dbo.t_umr (
   RID                  bigint               null,
   UUID                 bigint               null
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_user                                                */
/*==============================================================*/
create table dbo.t_user (
   IDENTIFICATION       bigint               identity(1, 1),
   USERNAME             varchar(50)          collate Chinese_PRC_CI_AS null,
   USERPWD              varchar(50)          collate Chinese_PRC_CI_AS null,
   EMPLOYEENAME         varchar(50)          collate Chinese_PRC_CI_AS null,
   ORGCODE              varchar(50)          collate Chinese_PRC_CI_AS null,
   ORGNAME              varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_user primary key (IDENTIFICATION)
         on "PRIMARY"
)
on "PRIMARY"
go
