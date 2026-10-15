# Escape Room 3D

Laboratorio Git + GitHub + Visual Studio Code + Unity.

## Equipo
- Yesenia (GitHub: yesenia548; nombre completo por confirmar).
- Alejandro (usuario de GitHub pendiente).
- Lucas (usuario de GitHub pendiente).
- Alex (usuario de GitHub pendiente).

## Version
Unity 6000.5.5f1, plantilla Universal 3D.

## Objetivo
Explorar una habitacion, encontrar la pista, resolver un codigo, abrir un cofre, obtener una llave y abrir la puerta de salida.

## Estructura de Assets
- 00_Scenes: MainScene.
- 01_Scripts: Player, Interaction, Puzzle, Environment y Systems.
- 02_Prefabs: Player, Chest, Key, Door, Desk y ClueNote.
- 03_Materials: materiales de suelo, paredes, madera, metal y paneles.
- 04_Models: malla RoomBlock.
- 05_Textures: textura de baldosas generada localmente.
- 06_UI: controles; interfaz de juego implementada en GameManager.OnGUI.
- Editor: preparacion, generacion de assets y validacion.

## Abrir y generar el contenido
1. Abrir este proyecto con Unity 6000.5.5f1 y esperar la importacion.
2. El generador prepara los assets la primera vez que compila. Si aun no aparecen, usar Escape Room > Crear escenario completo. Ese comando reconstruye MainScene: guardar primero cualquier trabajo propio.
3. Abrir Assets/00_Scenes/MainScene.unity.
4. Usar Escape Room > Validar assets y referencias, revisar Console y despues pulsar Play.

## Como jugar
Haz clic en Game para capturar el cursor. WASD mueve, el mouse gira la vista, E interactua y Esc libera el cursor. Lee la pista del escritorio, introduce 1234 en el panel, abre el cofre, recoge la llave, abre la puerta y cruza el umbral. La pantalla final permite reiniciar.

## Estado
Assets generados y compilados en Unity. MainScene ejecutada en Play. Prueba automatizada de estados: 11 comprobaciones PASS, incluida la salida. Prueba manual de teclado, mouse y raycast pendiente. Casos Git pendientes. No hay repositorio remoto conectado todavia.

## Evidencias
Registrar problema, proceso, solucion y prueba real para cada caso. No marcar como finalizado un caso sin su punto de control.


Movimiento documentado durante caso 12.
