create database flow
go
use flow
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_actor')
            and   type = 'U')
   drop table dbo.t_actor
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_command')
            and   type = 'U')
   drop table dbo.t_command
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_group')
            and   type = 'U')
   drop table dbo.t_group
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_instance')
            and   type = 'U')
   drop table dbo.t_instance
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_node')
            and   type = 'U')
   drop table dbo.t_node
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_process')
            and   type = 'U')
   drop table dbo.t_process
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.t_transition')
            and   type = 'U')
   drop table dbo.t_transition
go

execute sp_revokedbaccess dbo
go

/*==============================================================*/
/* User: dbo                                                    */
/*==============================================================*/
execute sp_grantdbaccess dbo
go

/*==============================================================*/
/* Table: t_actor                                               */
/*==============================================================*/
create table dbo.t_actor (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   RNID                 varchar(50)          collate Chinese_PRC_CI_AS null,
   IDENTIFICATION       bigint               null,
   APPELLATION          varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   CREATEDATETIME       datetime             null constraint DF_t_actor_INSERTDATE default getdate(),
   OPERATION            varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_actor primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_command                                             */
/*==============================================================*/
create table dbo.t_command (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   RNID                 varchar(50)          collate Chinese_PRC_CI_AS null,
   APPELLATION          varchar(50)          collate Chinese_PRC_CI_AS null,
   SCRIPT               varchar(4000)        collate Chinese_PRC_CI_AS null,
   CONNECTE             varchar(512)         collate Chinese_PRC_CI_AS null,
   DBCATEGORY           varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   COMMANDTYPE          varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_command primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go

execute sp_addextendedproperty 'MS_Description', 
   '文本',
   'user', 'dbo', 'table', 't_command', 'column', 'SCRIPT'
go

execute sp_addextendedproperty 'MS_Description', 
   '连接字符串',
   'user', 'dbo', 'table', 't_command', 'column', 'CONNECTE'
go

/*==============================================================*/
/* Table: t_group                                               */
/*==============================================================*/
create table dbo.t_group (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   RNID                 varchar(50)          collate Chinese_PRC_CI_AS null,
   IDENTIFICATION       bigint               null,
   APPELLATION          varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_role primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_instance                                            */
/*==============================================================*/
create table dbo.t_instance (
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS not null,
   CREATEDATETIME       datetime             null constraint DF_t_instance_CreateDateTime default getdate(),
   RNID                 bigint               null,
   FLOWID               varchar(50)          collate Chinese_PRC_CI_AS null,
   STATE                varchar(50)          collate Chinese_PRC_CI_AS null constraint DF_t_instance_STATUS default 'running',
   JSSTRUCTURE          text                 collate Chinese_PRC_CI_AS null,
   constraint PK_t_instance primary key (INSTANCEID)
         on "PRIMARY"
)
on "PRIMARY"
go

execute sp_addextendedproperty 'MS_Description', 
   '流程状态（运行中：running、结束：end、终止：termination,kill:杀死流程）',
   'user', 'dbo', 'table', 't_instance', 'column', 'STATE'
go

/*==============================================================*/
/* Table: t_node                                                */
/*==============================================================*/
create table dbo.t_node (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   IDENTIFICATION       bigint               not null,
   APPELLATION          varchar(50)          collate Chinese_PRC_CI_AS null,
   NODETYPE             varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_node primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_process                                             */
/*==============================================================*/
create table dbo.t_process (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   ORIGIN               bigint               null,
   DESTINATION          bigint               null,
   TRANSITIONID         varchar(50)          collate Chinese_PRC_CI_AS null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   NODETYPE             varchar(50)          collate Chinese_PRC_CI_AS null,
   CREATEDATETIME       datetime             null constraint DF_t_process_INSERTDATE default getdate(),
   RNID                 varchar(50)          collate Chinese_PRC_CI_AS null,
   OPERATION            varchar(50)          collate Chinese_PRC_CI_AS null constraint DF_t_process_OPERATE default 'normal',
   constraint PK_t_process primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: t_transition                                          */
/*==============================================================*/
create table dbo.t_transition (
   NID                  varchar(50)          collate Chinese_PRC_CI_AS not null,
   RNID                 varchar(50)          collate Chinese_PRC_CI_AS null,
   APPELLATION          varchar(128)         collate Chinese_PRC_CI_AS null,
   DESTINATION          bigint               null,
   ORIGIN               bigint               null,
   INSTANCEID           varchar(50)          collate Chinese_PRC_CI_AS null,
   EXPRESSION           varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK_t_transition_1 primary key (NID)
         on "PRIMARY"
)
on "PRIMARY"
go
