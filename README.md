🔧 AutoCAD Layer Matcher Plugin
A custom AutoCAD plugin built with WPF and the AutoCAD .NET API to streamline the process of assigning selected graphical objects to specific layers—enhancing speed, accuracy, and consistency in CAD workflows.

🎯 Purpose
Managing layer standards in detailed or large-scale AutoCAD projects can be a repetitive and error-prone task. This plugin introduces a more efficient method by allowing users to:

Select multiple objects in the AutoCAD drawing

Quickly match them to a specified layer using a custom interface

💡 Features
🖱️ Interactive UI: Built with WPF for a clean and responsive user experience

⚙️ AutoCAD Integration: Uses the AutoCAD .NET API for seamless interaction with drawings

🧱 MVVM Architecture: Ensures maintainability and clear separation of concerns

🚀 Getting Started
Requirements
AutoCAD (compatible version)

.NET Framework (target version used in the plugin)

Visual Studio (for development or customization)

Installation
Clone or download the repository

Open and build the solution in Visual Studio

Launch AutoCAD and run NETLOAD

Select the generated .dll file

Run the custom command (e.g., MatchLayerTool) to activate the plugin

🧪 How It Works
Launch the plugin via the custom command

Use the interface to choose a target layer

Select the objects you want to reassign

Click to apply the changes instantly

📸 Demo (Optional)
Add screenshots or a demo video here to showcase the UI and workflow.

📦 Technologies Used
WPF (.NET)

AutoCAD .NET API

MVVM Design Pattern
