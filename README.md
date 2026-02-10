# 📖 BibleQuery - Sistema de Gestión y Consulta Bíblica

## 💡 Descripción

 es una aplicación de escritorio robusta diseñada para la gestión, consulta y estudio de textos bíblicos.

El núcleo del proyecto es una base de datos relacional en **SQL Server** altamente normalizada (3FN), diseñada para manejar la gestión de usuarios con seguridad e historiales de actividad. La interfaz fue construida con **C# .NET (Windows Forms)**.
---

## 🛠️ Stack Tecnológico & Arquitectura

* **Base de Datos:** Microsoft SQL Server.
* **Backend Logic:** 100% 
* **Frontend:** C# .NET Framework (Windows Forms).
* **Herramientas:** Visual Studio, SQL Server Management Studio (SSMS).
* **Modelado:** Diagramas Entidad-Relación y Normalización hasta la Tercera Forma Normal (3NF).

---

## 🚀 Funcionalidades Principales

### 🔐 1. Gestión Avanzada de Usuarios
* **Autenticación Segura:** Login validado contra base de datos.
* **Políticas de Contraseña:** Validación de historial de contraseñas (no se pueden repetir las últimas 2) y requisitos de complejidad (Mayúsculas, caracteres especiales).
* **Auditoría:** Registro automático de fechas de alta, baja lógica y cambios de estatus

### 📖 2. Motor de Consulta Bíblica
* Navegación jerárquica optimizada: *Idioma -> Versión -> Testamento -> Libro -> Capítulo -> Versículo*.
* Visualización dinámica de pasajes con opciones de personalización (tamaño de fuente, idioma).

### 🔍 3. Búsqueda y Filtros
* Búsqueda de palabras clave en toda la Biblia o filtrada por Testamento/Libro.
* **Historial de Búsquedas:** El sistema guarda automáticamente qué buscó cada usuario y cuándo, permitiendo retomar consultas anteriores.

### ⭐ 4. Favoritos y Personalización
* Sistema CRUD para guardar versículos favoritos.
* Gestión personalizada por usuario.

---

## 🧠 Ingeniería de Datos

* **Normalización:** Base de datos estructurada en **3FN** para evitar redundancia (tablas separadas para `Géneros`, `Estatus`, `HistorialContraseñas`).
* **Stored Procedures:** Toda la interacción (INSERT, UPDATE, DELETE, SELECT) se realiza mediante procedimientos almacenados para seguridad y rendimiento.
    * Validar reglas de negocio antes de insertar.
    * Automatizar el historial de cambios de contraseña.
    * Gestionar bajas lógicas de usuarios.
---

## 📸 Capturas de Pantalla

| Login y Registro | Consulta Bíblica |
|:---:|:---:|
| ![Login](./Img_Interfaz/4.jpg) ![Registro](./Img_Interfaz/6.jpg) | ![Consultar](./Img_Interfaz/9.jpg) | ![Login](./Img_Interfaz/5.jpg) ![Login](./Img_Interfaz/7.jpg) ![Login](./Img_Interfaz/8.jpg)

| Búsqueda Avanzada, Favoritos | Gestión de Perfil y Preferencias|
|:---:|:---:|
| ![busqueda](./Img_Interfaz/11.jpg) ![Favoritos](./Img_Interfaz/10.jpg) ![Historial](./Img_Interfaz/12.jpg) | ![Login](./Img_Interfaz/4.jpg) |

*(Nota: Las imágenes representan la interfaz funcional desarrollada en Windows Forms)*

---


## 👥 Equipo de Desarrollo

* **José Armando Hernández Santander** - *Full Stack Developer*
* **Andrea Berenice Reyna Gutiérrez** - *Full Stack Developer*

---

*Este proyecto es de fines académicos y demostrativos para portafolio.*
