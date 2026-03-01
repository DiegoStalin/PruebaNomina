--Integracion Software
--Base da Datos Nomina 

-- DDL de la DB Nómina:
create database Nomina;
use Nomina;

CREATE TABLE departments (
dept_no INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
dept_name NVARCHAR(50) NOT NULL);

CREATE TABLE employees (
emp_no INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
ci NVARCHAR(50) NOT NULL,
birth_date DATE NOT NULL,
first_name NVARCHAR(50) NOT NULL,
last_name NVARCHAR(50) NOT NULL,
gender CHAR(1) NOT NULL CHECK (gender IN ('M', 'F')),
hire_date DATE NOT NULL,
correo NVARCHAR(50)
);

CREATE TABLE dept_emp (
emp_no INT NOT NULL,
dept_no INT NOT NULL,
from_date DATE NOT NULL,
to_date DATE NOT NULL,
CONSTRAINT Pk_Detalle_emp PRIMARY KEY (emp_no, dept_no, from_date),
CONSTRAINT Fk_Detalle_emp
	FOREIGN KEY (emp_no) REFERENCES employees (emp_no),
CONSTRAINT Fk_Detalle_dept
	FOREIGN KEY (dept_no) REFERENCES departments (dept_no)
);

CREATE TABLE dept_manager(
emp_no INT NOT NULL,
dept_no INT NOT NULL,
from_date DATE NOT NULL,
to_date DATE NOT NULL,
CONSTRAINT Pk_Detalle_manager PRIMARY KEY (emp_no, dept_no, from_date),
CONSTRAINT Fk_Detalle_manager_emp
	FOREIGN KEY (emp_no) REFERENCES employees (emp_no),
CONSTRAINT Fk_Detalle_manager_dept
	FOREIGN KEY (dept_no) REFERENCES departments (dept_no)
);

CREATE TABLE salaries (
emp_no INT NOT NULL,
salary BIGINT NOT NULL,
from_date DATE NOT NULL,
to_date DATE NULL,
CONSTRAINT Pk_Detalle_salaries PRIMARY KEY (emp_no, from_date),
CONSTRAINT Fk_Detalle_salaries
	FOREIGN KEY (emp_no) REFERENCES employees (emp_no)
);

CREATE TABLE titles (
emp_no INT NOT NULL,
title VARCHAR(50) NOT NULL,
from_date DATE  NOT NULL,
to_date DATE NULL,
CONSTRAINT Pk_detalle_titles PRIMARY KEY (emp_no, title, from_date),
CONSTRAINT Fk_detalle_titles
	FOREIGN KEY (emp_no) REFERENCES employees (emp_no)
);

CREATE TABLE users (
emp_no INT NOT NULL,
usuario VARCHAR(50) NOT NULL,
clave VARCHAR(50) NOT NULL,
CONSTRAINT Pk_users PRIMARY KEY (emp_no),
CONSTRAINT Uq_usuario UNIQUE (usuario),
CONSTRAINT FK_Detalle_users
	FOREIGN KEY (emp_no) REFERENCES employees (emp_no)
);

-- DML de la DB Nómina (10 registros por tabla):
-- Tabla departments:
INSERT INTO departments (dept_name)
VALUES	('Ventas'),
		('TICs'),
		('Gerencia General'),
		('Adquisiciones'),
		('Financiero'),
		('Logistica'),
		('RRHH'),
		('Contratación Pública'),
		('Jurídico'),
		('Investigación'),
		('Comercio Exterior');

-- Tabla employees:
INSERT INTO employees (ci, birth_date, first_name, last_name, gender, hire_date, correo)
VALUES	
('1715658795', '1985-05-10', 'Juan', 'Lopez', 'M','2020-02-03', 'juanlopez@nomina.com'),
('1723456789', '1990-03-12', 'Carlos', 'Mena', 'M', '2018-01-15', 'cmena@nomina.com'),
('0912345678', '1988-07-22', 'María', 'Pérez', 'F', '2019-04-10', 'mperez@nomina.com'),
('1102345678', '1992-11-05', 'Luis', 'Andrade', 'M', '2020-06-01', 'landrade@nomina.com'),
('0803456789', '1985-02-18', 'Ana', 'Torres', 'F', '2017-09-20', 'atorres@nomina.com'),
('0604567890', '1995-08-30', 'Diego', 'Vera', 'M', '2021-03-12', 'dvera@nomina.com'),
('1705678901', '1983-12-14', 'Patricia', 'Ríos', 'F', '2016-11-01', 'prios@nomina.com'),
('1006789012', '1991-05-09', 'Jorge', 'Salazar', 'M', '2019-08-19', 'jsalazar@nomina.com'),
('1407890123', '1987-10-25', 'Lucía', 'Morales', 'F', '2018-02-05', 'lmorales@nomina.com'),
('0308901234', '1994-06-17', 'Andrés', 'Castillo', 'M', '2022-01-10', 'acastillo@nomina.com');

--Tabla dept_emp:
INSERT INTO dept_emp (emp_no, dept_no, from_date, to_date)
VALUES	
(1, 1,'2020-02-03', '9999-12-31'),
(2, 2, '2018-01-15', '2020-12-31'),
(2, 5, '2021-01-01', '9999-12-31'),

(3, 7, '2019-04-10', '9999-12-31'),

(4, 6, '2020-06-01', '2022-05-31'),
(4, 2, '2022-06-01', '9999-12-31'),

(5, 4, '2017-09-20', '2020-08-31'),
(5, 1, '2020-09-01', '9999-12-31'),

(6, 2, '2021-03-12', '9999-12-31'),

(7, 3, '2016-11-01', '2021-12-31'),
(7, 3, '2022-01-01', '9999-12-31'),

(8, 6, '2019-08-19', '9999-12-31'),

(9, 8, '2018-02-05', '2021-06-30'),
(9, 10,'2021-07-01', '9999-12-31'),

(10, 2, '2022-01-10', '9999-12-31');

--Tabla dept_manager:
INSERT INTO dept_manager (emp_no, dept_no, from_date, to_date)
VALUES	
(1, 1,'2023-02-03', '9999-12-31'),
(7, 3, '2020-01-01', '9999-12-31'),
(5, 1, '2022-01-01', '9999-12-31'),
(2, 5, '2023-01-01', '9999-12-31'),
(3, 7, '2021-01-01', '2023-12-31'),   
(4, 6, '2022-01-01', '9999-12-31'),           
(6, 2, '2023-03-01', '9999-12-31'),          
(8, 6, '2020-01-01', '2022-12-31'),   
(9, 8, '2021-01-01', '9999-12-31'),          
(10,2, '2024-01-01', '9999-12-31'); 


--Tabla salaries:
INSERT INTO salaries (emp_no, salary, from_date, to_date)
VALUES	
(1, 2000,'2023-02-03', '9999-12-31'),
(2, 1200, '2018-01-15', '2020-12-31'),
(2, 1800, '2021-01-01', '9999-12-31'),

(3, 1300, '2019-04-10', '9999-12-31'),

(4, 1100, '2020-06-01', '2022-05-31'),
(4, 1500, '2022-06-01', '9999-12-31'),

(5, 1400, '2017-09-20', '2020-08-31'),
(5, 2000, '2020-09-01', '9999-12-31'),

(6, 1000, '2021-03-12', '9999-12-31'),

(7, 2500, '2016-11-01', '2021-12-31'),
(7, 3000, '2022-01-01', '9999-12-31'),

(8, 1200, '2019-08-19', '9999-12-31'),

(9, 1350, '2018-02-05', '2021-06-30'),
(9, 1600, '2021-07-01', '9999-12-31'),

(10, 950, '2022-01-10', '9999-12-31');

--Tabla titles:
INSERT INTO titles (emp_no, title, from_date, to_date)
VALUES	
(1, 'Jefe Jurídico','2023-02-03', '9999-12-31'),
(2, 'Analista TIC', '2018-01-15', '2020-12-31'),
(2, 'Administrador de Sistemas', '2021-01-01', '9999-12-31'),

(3, 'Asistente RRHH', '2019-04-10', '9999-12-31'),

(4, 'Auxiliar Logístico', '2020-06-01', '2022-05-31'),
(4, 'Coordinador Logístico', '2022-06-01', '9999-12-31'),

(5, 'Asesor Comercial', '2017-09-20', '2020-08-31'),
(5, 'Jefe de Ventas', '2020-09-01', '9999-12-31'),

(6, 'Soporte Técnico', '2021-03-12', '9999-12-31'),

(7, 'Director General', '2016-11-01', '9999-12-31'),

(8, 'Analista Financiero', '2019-08-19', '9999-12-31'),

(9, 'Abogada Junior', '2018-02-05', '2021-06-30'),
(9, 'Abogada Senior', '2021-07-01','9999-12-31'),

(10, 'Asistente TIC', '2022-01-10', '9999-12-31');

--Tabla users:
INSERT INTO users (emp_no, usuario, clave)
VALUES	
(1, 'jlopez','jlopez1985'),
(2, 'cmena', 'cmena1990'),
(3, 'mperez', 'mperez1988'),
(4, 'landrade', 'landrade1992'),
(5, 'atorres', 'atorres1985'),
(6, 'dvera', 'dvera1995'),
(7, 'prios', 'prios1983'),
(8, 'jsalazar', 'jsalazar1991'),
(9, 'lmorales', 'lmorales1987'),
(10,'acastillo', 'acastillo1994');


select 
e.first_name + e.last_name as 'Nombre y Apellido',
--e.hire_date,
d.dept_name
from employees e
inner join dept_emp de
on e.emp_no=de.emp_no
inner join departments d
on de.dept_no = d.dept_no;
--where de.to_date='9999-12-31';

-- mostrar todos los empleados con su departamento actual

select
e.first_name + e.last_name as 'Nombre y Apellido',
e.hire_date as 'fecha contratación',
de.dept_name
from employees e
inner join dept_emp d
on
e.emp_no=d.emp_no
inner join departments de
on 
d.dept_no=de.dept_no
where d.to_date='9999-12-31'

-- historial cargo y salario
select
emp.first_name + ' '+ emp.last_name as 'Nombres Completos',
tit.title as 'Cargo Actual',
sal.salary as 'Salario'
from employees emp
inner join titles tit
on emp.emp_no=tit.emp_no

inner join salaries sal
on emp.emp_no=sal.emp_no

where tit.to_date = '9999-12-31' 
and sal.to_date='9999-12-31';


-- jefes por departamento

select
depto.dept_name,
e.first_name + ' '+e.last_name as 'Nombre Completo de Jefe Actual',
mang.from_date as 'Fecha inicio como Jefe'
from departments depto
inner join dept_manager mang
on depto.dept_no = mang.dept_no

inner join employees e
on mang.emp_no = e.emp_no
and mang.dept_no = depto.dept_no

where mang.to_date='9999-12-31';



---
--necesidad: el area de rrhh necesita conocer que empleados actualmente pertenecen a cada departamento
-- junto con su fecha de ingreso y correo. (ci, nombre, correo, depto, fecha ingfreso)
-- debe estar ordenado por fecha de ingreso

select
emp.ci,
emp.first_name +' '+ emp.last_name as 'Nombre Completo',
emp.hire_date as 'Fecha Contratacion',
depa.dept_name

from employees emp
inner join dept_emp dept
on emp.emp_no=dept.emp_no

inner join departments depa
on depa.dept_no=dept.dept_no
where dept.to_date='9999-12-31';


----
--necesidad: el departamento financiero requiere idientificar a los empleados (ci, nombre completo, salario)
-- que ganan mas que el promedio actual de salarios para evaluar reajustes
select
emp.first_name,
emp.ci,
emp.first_name +' '+ emp.last_name as 'Nombre Completo',
sal.salary as 'Salario'
from employees emp

inner join salaries sal
on sal.emp_no=emp.emp_no
where sal.salary> (select AVG(sal2.salary) from salaries sal2)
order by sal.salary;


---
--left join ausencias de relacion
 
select
emp.first_name +' '+ emp.last_name as NombreCompleto,
us.usuario
from users us
left join employees emp
on us.emp_no=emp.emp_no
order by emp.first_name;


---
--join con filtro temmporal (consulta historica)
-- mostrar quiene era jefe en eun fecha especifica 2021-06-01
select
depa.dept_name,
emp.first_name + ' '+ emp.last_name as Nombre_Completo,
mang.from_date as Fecha_Inicio,
mang.to_date as FechaFin
from departments depa
inner join dept_manager mang 
on depa.dept_no= mang.dept_no
inner join employees emp
on emp.emp_no=mang.emp_no
where '2021-06-01' between mang.from_date and mang.to_date;


-- JOIN con multiples historiales
select
emp.first_name+ ' '+emp.last_name as Nombre_Completo,
depart.dept_name as Departamento_Actual,
tit.title as Cargo_Actual,
sal.salary as Salario_Actual
from employees emp
inner join dept_emp de
on emp.emp_no=de.emp_no
inner join departments depart
on depart.dept_no=de.dept_no
inner join titles tit
on tit.emp_no=de.emp_no

inner join salaries sal
on sal.emp_no=de.emp_no
where sal.to_date ='9999-12-31'
and tit.to_date='9999-12-31'
and de.to_date='9999-12-31';


-- join inverso quien no cumple una condicion
-- listar los empleados que no son jefes de ningun departamento

INSERT INTO employees (ci, birth_date, first_name, last_name, gender, hire_date, correo)
VALUES
('1111111111', '1996-04-20', 'Paola', 'Cevallos', 'F', '2023-05-01', 'pcevallos@nomina.com');
INSERT INTO dept_emp (emp_no, dept_no, from_date, to_date)
VALUES
(11, 4, '2023-05-01', '9999-12-31');
INSERT INTO salaries (emp_no, salary, from_date, to_date)
VALUES
(11, 900, '2023-05-01', '9999-12-31');
INSERT INTO titles (emp_no, title, from_date, to_date)
VALUES
(11, 'Asistente Administrativo', '2023-05-01', '9999-12-31');

select
emp.first_name + ' ' +emp.last_name as Empleados_que_no_son_jefes

from employees emp
left join dept_manager man
on emp.emp_no=man.emp_no 
where man.dept_no is NULL

select
depa.dept_name as Departamento
from dept_manager man
right join departments depa
on depa.dept_no=man.dept_no 
where man.dept_no is NULL

--join con agregacion
-- mostrar cada departamento con cantidad de empleados actuales y nombre del jefe actual
use Nomina
select
depa.dept_name as Departamento,
emp.first_name + ' ' + emp.last_name as Jefe_Actual,
count(de.emp_no) as Total_Empleados_por_Departamento 
from departments depa
inner join dept_manager man
on depa.dept_no=man.dept_no
inner join employees emp
on emp.emp_no=man.emp_no
inner join dept_emp de
on de.dept_no=man.dept_no
and de.to_date = '9999-12-31'
and man.to_date = '9999-12-31'
group by depa.dept_name, emp.first_name, emp.last_name


-- empleados sin usuario
-- mostrar los empleados que no tienen usuario asignados
select
emp.first_name + ' '+ emp.last_name as Nombre_Empleado_sin_Usuario
from employees emp
left join users u
on u.emp_no= emp.emp_no
where u.emp_no is null;

-- usuarios sin empleado
-- listar los usuarioos que no estan asociados a ningun empleado
select
u.usuario as Usuarios_sin_Empleado
from users u
left join employees emp
on u.emp_no=emp.emp_no
where emp.emp_no is null



-- jefes actuales con departamento
-- mostrar nombre de depto, nombre completo de jefe, fecha de inicio

select 
depa.dept_name as Departamento,
emp.first_name + ' '+emp.last_name as Nombre_completo,
mana.from_date as Fecha_Inicio
from departments depa
inner join dept_manager mana
on depa.dept_no=mana.dept_no
and mana.to_date = '9999-12-31'
inner join employees emp
on emp.emp_no=mana.emp_no;



-- empleados y su departamento actual
-- mostrar todos los empleados con sus departamento actual
-- si un empleado no tiene departamento vigente igual debe aparecer

select 
emp.first_name+' '+emp.last_name as Empleado,
depa.dept_name as Departamento
from employees emp
left join dept_emp de
on emp.emp_no=de.emp_no
and de.to_date = '9999-12-31'
left join departments depa
on depa.dept_no=de.dept_no
;

-- empleados que han sido jefes
-- listar los empleados que alguna vez fueron jefes de depto

select distinct 
emp.first_name+' '+emp.last_name as Jefe_alguna_vez
from employees emp
inner join dept_manager man
on emp.emp_no=man.emp_no

-- conteo por departamento
-- mostrar departamento, cantidad de empleados actuales
select 
depa.dept_name as Departamento,
count (de.emp_no) as Cantidad
from departments depa
left join dept_emp de
on depa.dept_no=de.dept_no
and de.to_date='9999-12-31'
group by depa.dept_name



--- CLASE 29 ENERO 2026

-- Procedimientos Almacenados y/o Store Procedures
-- al igual que otros objetos son bloques de codigo,(metodos, funicones programas) realizados en sql
-- que se almacenan de forma fisica en una base de datos y se pueden ejecutar de manera simultanea
-- y/o repetitiva dentro de un sistema.

-- cuando trabajo con operaciones transcaccionales interactuo con los datos por medio de la app
-- y estos me ayduan a que la info este mejor gestionada, evita cuellos de botella en la info.

-- VENTAJAS
-- Reutilizacion el mismo bloque de codigo con una reutiulizacion diferente entrega la info que se neesita
-- Optimizacion, solo se necesita programarlo una ocacion, optimiza tiempo y recursos
-- genere todas las logicas de negocio. estop funciona en oracle o sql server la logica se hace en la base de datos
-- por lo que el tiempo de respuesta es mas rapido porque no se realiza en la aplicacion.


-- SINTAXIS
-- DDL porque son objetos

--PARA CREAR:
/* 
CREATE PROCEDURE sp_Nombre
AS
	BEGIN
		--CODIGO SQL
	END

GO
*/

--PARA MODIFICAR
/*
ALTER PROCEDURE sp_Nombre
AS
	BEGIN
		--CODIGO SQL
	END

GO
*/

-- ELIMINAR CON DROP
-- EJECUTAR CON EXEC

--EJEMPLO
-- NECESIDAD: todas las semanas se requieren la lista de 
-- lo empleados de la compania ( nombre cedula correro fecha ingreso)

CREATE PROCEDURE sp_listarEmpleadosSemanal
as

	begin
		select * from employees
	end
go

use Nomina;

alter PROCEDURE sp_listarEmpleadosSemanal
as

	begin
		select first_name+' '+ last_name 'NOmbre',
ci 'cedula', correo, hire_date 'fecha ingreso'
from employees
	end
go

exec [dbo].[sp_listarEmpleadosSemanal];

-- Necesidad. se desea conocer la info laboral de un empleado
-- nombre,cedula, fecha ingreso, nombre departamento,. cargo, salario
-- donde la busqueda nace del numero de cedula.
go
CREATE PROCEDURE sp_informacionlaboralempleado
@ci varchar (50)
--@nombre varchar(50)
as

	begin
		select e.ci, e.first_name + ' ' + e.last_name Nombre, e.correo,
		(select d.dept_name
		from departments d join dept_emp de on d.dept_no=de.dept_no
		where de.emp_no=(select emp_no from employees where ci=@ci)) Departamento,
		t.title Cargo, s.salary Salario, e.hire_date 'Fecha de Ingreso'
		from employees e
		join dept_emp de
		on e.emp_no=de.emp_no
		join titles t
		on e.emp_no=t.emp_no
		join salaries s
		on e.emp_no=s.emp_no
		where e.ci=@ci
	end
go
use Nomina
Exec [dbo].[sp_informacionlaboralempleado] '1715658795'

--necesidad
-- validar la existencia o no de un empleado a partir de numero de cedula
-- si el empleado no existe hay que registrarlo en el sistema y mostrar un mensaje "empelado registrado"
-- y mostrar la informacion registrada

-- si el empleado ya esiute, se debe mostrar un mensaje "empleado ya existe en el sistema"

-- analisis
-- buscar el empleado a traves del filtro
-- condicion
	--si: "empleado ya existe en el sistema"
	--no: "se debe registrar la info del empelado y mostrar la info y un sms "empleado registrado"

CREATE PROCEDURE sp_ValidarExistenciaEmpleado
@ci varchar (50),
@birth_date varchar(50),
@first_name varchar(50),
@last_name varchar(50),
@gender char,
@hire_date date,
@correo varchar(50),
@mensaje varchar(100) output --este es un parametro de salida

as

	begin
		--condicion
		if (not exists (select * from employees where ci=@ci))
			begin
				--registrar el empleado
				insert into employees (ci, birth_date, first_name, last_name, gender, hire_date, correo)
				values (@ci, @birth_date, @first_name, @last_name, @gender, @hire_date, @correo)

				--mostrar mensaje
				set @mensaje='Empleado Registrado'

				select * from employees where ci=@ci
			end
		else
			begin
				set @mensaje='Empelado ya existe en el sistema'
			end
	end
go

--para ejecutar. necesito variable para resultado de la ejecucion del objeto

begin
	declare @mensaje varchar (100)
	exec sp_ValidarExistenciaEmpleado '165434231', '','','','','', @mensaje output
	select @mensaje
end

-- EJERCICIO EN CLASE: 
-- Insertar employee + user + salary + title + dept_emp en una sola transacción.

CREATE PROCEDURE sp_InsertarTodo
(
    @ci NVARCHAR(50),
    @birth_date DATE,
    @first_name NVARCHAR(50),
    @last_name NVARCHAR(50),
    @gender CHAR(1),
    @hire_date DATE,
    @correo NVARCHAR(50),

    @usuario VARCHAR(50),
    @clave VARCHAR(50),

    @salary BIGINT,
    @title VARCHAR(50),

    @dept_no INT,
    @from_date DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @emp_no INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- EMPLOYEE
        INSERT INTO employees
        (ci, birth_date, first_name, last_name, gender, hire_date, correo)
        VALUES
        (@ci, @birth_date, @first_name, @last_name, @gender, @hire_date, @correo);

        SET @emp_no = SCOPE_IDENTITY();

        -- USERS
        INSERT INTO users
        (emp_no, usuario, clave)
        VALUES
        (@emp_no, @usuario, @clave);

        -- SALARIES
        INSERT INTO salaries
        (emp_no, salary, from_date, to_date)
        VALUES
        (@emp_no, @salary, @from_date, '9999-12-31');

        -- TITLES
        INSERT INTO titles
        (emp_no, title, from_date, to_date)
        VALUES
        (@emp_no, @title, @from_date, '9999-12-31');

        -- DEPT_EMP
        INSERT INTO dept_emp
        (emp_no, dept_no, from_date, to_date)
        VALUES
        (@emp_no, @dept_no, @from_date, '9999-12-31');

        COMMIT;

        PRINT 'EMPLEADO INSERTADO CORRECTAMENTE';
    END TRY
    BEGIN CATCH
        ROLLBACK;
        PRINT 'ERROR EN LA TRANSACCIÓN';
        THROW;
    END CATCH
END;
GO


EXEC sp_InsertarTodo
    @ci = '1011111111',
    @birth_date = '1994-09-27',
    @first_name = 'Fabricio',
    @last_name = 'Llumiquinga',
    @gender = 'M',
    @hire_date = '2026-02-01',
    @correo = 'jose.lga@ister.edu.ec',
    @usuario = 'fabrijlle',
    @clave = '1234',
    @salary = 100,
    @title = 'Docente',
    @dept_no = 5,
    @from_date = '2026-01-01';

SELECT * FROM employees;


-- EJERCICIO 1 propio
-- mostrar todos los empleados con: 

create procedure sp_mostrar_todos_empleados

as
	BEGIN
		SELECT emp_no, first_name + ' '+ last_name as Nombre_Completo, correo, hire_date
		from employees
	END
EXEC [dbo].[sp_mostrar_todos_empleados];

-- EJERCICIO 2 propio
-- listar empleados contratados despues del 2020:

create procedure sp_emp_contratados_d2020
as
	begin 
	set nocount on;
		select 
		emp_no, ci, first_name, last_name, hire_date, correo
		from employees
		where hire_date>'2020-01-01'
	end

-- EJERCICIO 3 propio
-- mostrar solo empleados hombres

create procedure sp_emp_hombre
as
	begin
	set nocount on;
		select emp_no, ci, first_name, last_name, hire_date, correo
		from employees
		where gender='M'
	end

-- BLOQUE 2: JOINS (MUY importante)
-- ejercicio 4
-- mostrar: emp_no, nombre completo, departamento actual

-- clase 5 feb
select name 
from sys.procedures -- 
where schema_id=SCHEMA_ID('dbo');
-- vista par alos salarios superiores a 1000


create view Salarios_mayores1000 
as
select emp.first_name + ' ' + emp.last_name Nombre_Completo,
sal.salary as Salario
from employees emp
inner join salaries sal
on emp.emp_no= sal.emp_no
where sal.salary>1000
and sal.to_date='9999-12-31'

select * from Salarios_mayores1000

-- una vista que me indique quienes tienen mas de 2 años trabajando
create view Trabaja_mas_2_anios
as
select emp.first_name + ' ' + emp.last_name Nombre_Empleado,
emp.hire_date Fecha_Ingreso
from employees emp
where emp.hire_date <= dateadd(year,-2, getdate())

select * from Trabaja_mas_2_anios

select getdate();


-- PROCEDIMIENTO PARA VALIDAR UN USUARIO
use Nomina
go

set ansi_nulls on
go
set quoted_identifier on
go
create procedure [dbo].[spValidarUsuario]

	@usuario varchar (100),
	@clave varchar (100),
	@id int output,
	@mensaje varchar (100) output
as
begin

	set nocount on;
	declare @IsStaff INT = NULL

	if (exists (select * from users u join employees e on u.emp_no =e.emp_no where e.correo=@usuario and u.clave=@clave))
	begin
		set @IsStaff =(select e.emp_no from users u join employees e on u.emp_no=e.emp_no where e.correo=@usuario and u.clave=@clave)
	end
	if @IsStaff is not null
	begin
		set @mensaje='Codigo 1. Autenticacion exitosa, El empleado existe'
		set @id=1
	end
	else begin
		set @mensaje='Codigo 0. El Usuario no existe o ha ingresado mal la clave.'
		set @id=0
	end

	select @id,@mensaje
end

-- PROCEDIMIENTO PARA EL REGISTRO
GO

set ansi_nulls on
go
set quoted_identifier on
go

alter procedure [dbo].[spRegistrarUsuario] 
(
@cedula varchar(50),
@nombre varchar(50),
@apellido varchar(50),
@correo varchar(100),
@genero char(1),
@fecha DATE,
@clave varchar (100),
@retorno int output,
@mensaje varchar(100) output
)
as
begin
	set nocount on;

	declare @pos1 int
	declare @pos2 int
	declare @usuario varchar (50)
	declare @id int
	declare @id_ultimoEmployess int

	if (not exists(select * from employees where correo=@correo and ci=@cedula))
	begin
		
		-- obtiene emp_no siguietnte
		--select @id_ultimoEmployess = max (emp_no) +1 from employees

		-- inserta nuevo empleado
		insert into employees(birth_date, first_name, last_name, gender, hire_date, correo, ci) 
		values ( getdate(), @nombre, @apellido, @genero, @fecha,  @correo, @cedula)
		
		-- para obtener id insertado
		select @id=emp_no from employees where correo=@correo and ci=@cedula

		-- genera usuario desde correo
		Select @pos1=CHARINDEX ('.', @correo)+1, 
		@pos2=CHARINDEX ('@', @correo)-@pos1,@usuario = CONCAT(SUBSTRING(@correo, 1,1), SUBSTRING(@correo, @pos1, @pos2))

		
		--inserta em la tabla users
		insert into users(emp_no, usuario, clave) values (@id, @usuario, @clave)

		set @retorno=1
		set @mensaje='El usuario se ha registrado con exito'

		select @retorno, @mensaje
	end
	else
	begin
		set @retorno=0
		set @mensaje='El usuario ya esta registrado'

		select @retorno, @mensaje
	end
end

select * from employees e join users u on e.emp_no=u.emp_no