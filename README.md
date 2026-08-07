\# 📋 Student Task Manager



A cross-platform student task management application built with \*\*.NET MAUI\*\*, designed to help students organize, track, and manage their academic and personal tasks.



The application provides task creation, editing, searching, filtering, sorting, authentication, task completion tracking, and notifications through a clean and user-friendly interface.



\---



\## 🚀 Features



\### 🔐 Authentication

\- User registration

\- Secure password hashing using BCrypt

\- User login

\- Logout functionality

\- Account/settings section



\### 📝 Task Management

\- Create new tasks

\- Edit existing tasks

\- Delete tasks

\- Mark tasks as completed

\- View task details

\- Set task categories

\- Set task priorities

\- Set task due dates



\### 🔎 Search, Filter \& Sort

\- Search tasks by title, description, or category

\- Filter tasks by category

\- Sort tasks by:

&#x20; - Due date

&#x20; - Priority

&#x20; - Title



\### 📊 Task Dashboard

The home dashboard provides an overview of:



\- Total tasks

\- Completed tasks

\- Pending tasks

\- Overdue tasks

\- High-priority tasks

\- Task completion progress



\### 🔔 Notifications

The application includes local notification functionality for task-related reminders.



\### 📱 User Interface

\- Responsive .NET MAUI interface

\- Dashboard-style home screen

\- Swipe actions for completing and deleting tasks

\- Empty-state messaging when no tasks exist

\- Application splash screen and branding



\---



\## 🛠️ Technologies



\- \*\*C#\*\*

\- \*\*.NET 8\*\*

\- \*\*.NET MAUI\*\*

\- \*\*XAML\*\*

\- \*\*SQLite\*\*

\- \*\*SQLite-net-pcl\*\*

\- \*\*CommunityToolkit.Maui\*\*

\- \*\*CommunityToolkit.Mvvm\*\*

\- \*\*BCrypt.Net-Next\*\*

\- \*\*Plugin.LocalNotification\*\*

\- \*\*Visual Studio 2022\*\*

\- \*\*Git \& GitHub\*\*



\---



\## 🏗️ Architecture



The application follows a structured architecture separating the user interface, business logic, data access, and application models.



```text

StudentTaskManager

│

├── Models

│   ├── User.cs

│   └── TaskItem.cs

│

├── Views

│   ├── LoginPage.xaml

│   ├── RegisterPage.xaml

│   ├── HomePage.xaml

│   ├── AddTaskPage.xaml

│   ├── EditTaskPage.xaml

│   └── SettingsPage.xaml

│

├── ViewModels

│   ├── LoginViewModel.cs

│   ├── RegisterViewModel.cs

│   ├── HomeViewModel.cs

│   ├── AddTaskViewModel.cs

│   ├── EditTaskViewModel.cs

│   └── SettingsViewModel.cs

│

├── Services

│   ├── DatabaseService.cs

│   ├── AuthenticationService.cs

│   └── NotificationService.cs

│

├── Resources

│   ├── AppIcon

│   ├── Splash

│   ├── Images

│   └── Fonts

│

├── App.xaml

├── AppShell.xaml

├── MauiProgram.cs

└── StudentTaskManager.csproj

