IF(DB_ID('MinionsDB') IS NOT NULL)
BEGIN
	alter database MinionsDB set single_user with rollback immediate;
	USE master;
	DROP DATABASE MinionsDB;
END
CREATE DATABASE MinionsDB;
use MinionsDB;