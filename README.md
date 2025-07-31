# Unity Custom Tools - Herramienta de Lenguaje para Unity

![Captura de Pantalla/GIF del Proyecto](ruta/a/tu/captura_o_gif.gif)
_Una captura de pantalla o un GIF corto de tu escena demo en acción._

Una herramienta de localización simple pero potente para Unity, diseñada para gestionar y cambiar fácilmente entre múltiples idiomas tanto para activos de texto como de audio dentro de tus proyectos de Unity.

---

## ✨ Características

* **Gestión Basada en ScriptableObject:** Datos de idioma centralizados usando **ScriptableObjects** para una fácil organización y edición.
* **Localización de Texto:** Localiza sin problemas todos los elementos de texto de la UI.
* **Localización de Audio:** Gestiona y reproduce clips de audio específicos para cada idioma.
* **Traducciones Indexadas:** Asegura que las traducciones de texto y audio para cada idioma se alineen perfectamente usando índices de array.
* **Fácil Integración:** Diseñado para una configuración rápida y cambios de código mínimos en proyectos existentes.

---

## 🚀 Empezando

### Instalación

Clona o descarga este repositorio en la carpeta `Assets` de tu proyecto Unity.
    ```bash
    git clone https://github.com/Caalpar/Unity-Custom-Tools.git 
    ```


### Cómo Usar

### 1. Crea tus Activos de Idioma

#### Create LanguagueManager:
  ![Create LanguagueManager](Assets/img/Tutorial1_1.jpg)
#### Create Languague:
  ![Create Languague](Assets/img/Tutorial1_2.jpg)
  
  (Crea uno para cada idioma que quieras soportar, por ejemplo, "English", "Spanish", "French").

#### 2. Configura tu Language Manager

1.  Selecciona tu ScriptableObject `LanguageManager` en la ventana del Proyecto.
2.  Arrastra y suelta tus ScriptableObjects `Language` en el array "Languages".
3.  Establece el `Default Language` al idioma que quieras cargar inicialmente.

#### 3. Añade Traducciones a Cada Idioma

1.  Selecciona un ScriptableObject `Language` (por ejemplo, "Spanish").
2.  En el Inspector, expande los arrays "Text Translations" y "Audio Translations".
3.  **Fundamentalmente, asegúrate de que el índice para un texto/audio específico coincida en todos los idiomas.**
    * Ej: `Text Translations[0]` = "Hola Mundo" (Español)
    * `Text Translations[0]` = "Hello World" (Inglés)
    * `Audio Translations[0]` = `audio_hola_espanol.mp3`
    * `Audio Translations[0]` = `audio_hello_english.mp3`

#### 4. Usando en Tu Escena

* **Para Texto:**
    1.  Añade un componente `LocalizedText` a tu GameObject `TextMeshProUGUI` o `UI.Text`.
    2.  Asigna el índice de texto deseado de tus ScriptableObjects `Language`.
    * _Ejemplo de fragmento de código para mostrar cómo podría verse/funcionar el componente `LocalizedText`._
        ```csharp
        // Ejemplo: En algún lugar de tu código cuando necesites actualizar texto
        public class MyLocalizedText : MonoBehaviour
        {
            [SerializeField] private int textIndex; // El índice para este texto específico
            private TextMeshProUGUI tmpro;

            void Awake()
            {
                tmpro = GetComponent<TextMeshProUGUI>();
                // Esto es un ejemplo simplificado. Tu sistema real probablemente
                // escuchará un evento del LanguageManager para cambios de idioma.
                UpdateText();
            }

            // Llama a esto cuando el idioma cambie
            public void UpdateText()
            {
                tmpro.text = LanguageManager.Instance.GetCurrentText(textIndex);
            }
        }
        ```

* **Para Audio:**
    1.  Añade un componente `LocalizedAudioSource` a un GameObject con un `AudioSource`.
    2.  Asigna el índice de audio deseado.
    * _Ejemplo de fragmento de código para audio._
        ```csharp
        // Ejemplo: En algún lugar de tu código cuando necesites reproducir audio
        public class MyLocalizedAudioPlayer : MonoBehaviour
        {
            [SerializeField] private int audioIndex; // El índice para este audio específico
            private AudioSource audioSource;

            void Awake()
            {
                audioSource = GetComponent<AudioSource>();
            }

            public void PlayLocalizedClip()
            {
                audioSource.clip = LanguageManager.Instance.GetCurrentAudioClip(audioIndex);
                audioSource.Play();
            }
        }
        ```

#### 5. Cambiando Idiomas en Tiempo de Ejecución

Puedes cambiar el idioma actual llamando a un método en tu `LanguageManager`.

```csharp
// Ejemplo: Desde un clic de botón de UI o lógica del juego
public void ChangeLanguageToEnglish()
{
    LanguageManager.Instance.SetLanguage("English"); // O por índice si se prefiere
}

public void ChangeLanguageToSpanish()
{
    LanguageManager.Instance.SetLanguage("Spanish");
}
