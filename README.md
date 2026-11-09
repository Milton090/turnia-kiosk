# Turnia Kiosco

Kiosco de autoservicio del prototipo Turnia, un sistema de gestión de turnos. Es una aplicación de escritorio para Windows hecha en .NET MAUI, donde la persona escoge un servicio y recibe su ticket, que se muestra en pantalla y se imprime si hay una impresora Bixolon conectada.

El proyecto se completa con [turnia-backend](https://github.com/Milton090/turnia-backend) y [turnia-frontend](https://github.com/Milton090/turnia-frontend).

## Descarga

La versión lista para usar está en [OneDrive](https://laiberocol-my.sharepoint.com/:u:/g/personal/mrami186_estudiante_ibero_edu_co/IQAGEsvJeQzGQIyBlcIudGc1AexwpaWIl6x1selaFJvSyKg?e=8HGDsY). No necesita instalación, basta con descomprimir el .zip y abrir `publish\TurniaKiosk.exe`. Esta versión se conecta a la API publicada.

## Cómo correrlo

Necesita el SDK de .NET 10 con el workload de MAUI para Windows y el backend corriendo en local. Conviene clonar el repositorio en una ruta corta, porque la compilación genera rutas largas y Windows tiene un límite de 260 caracteres.

```powershell
dotnet run --project TurniaKiosk\TurniaKiosk.csproj
```

En Debug el kiosco se conecta a http://localhost:6101.

## Configuración

La URL del backend y la clave del kiosco están en `TurniaKiosk/Services/Constants/ConstantDefinitions.cs`. En Debug se usa localhost y en Release la API publicada. La clave es `turnia-kiosco-demo`, que es la que crea el backend con sus datos de prueba.

## Generar el ejecutable

```powershell
dotnet publish TurniaKiosk\TurniaKiosk.csproj -c Release -f net10.0-windows10.0.19041.0
```

El resultado queda en `TurniaKiosk\bin\Release\net10.0-windows10.0.19041.0\win-x64\publish`. La publicación es self-contained, así que esa carpeta se puede comprimir y llevar a otro equipo sin instalar nada más.

## Impresora

Usa el SDK de Bixolon que viene en el proyecto, `BXLPAPI.dll` junto con `Services/Bixolon`. Sin impresora el ticket se crea igual, solo que no se imprime.

## Estructura

`Pages` y `ViewModels` tienen las pantallas y su lógica, `Services/Queue` el cliente de la API, `Services/Bixolon` la impresión y `Resources` los estilos, imágenes y fuentes.
