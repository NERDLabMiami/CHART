using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class VideoListing : MonoBehaviour
{
    // Individual listing components
    public TextMeshProUGUI title;       // Title of the video
    public TextMeshProUGUI description; // Description of the video
    public Image thumbnail;             // Thumbnail for the video
    public string videoHost = "https://nerdlab.miami/chart/360/";

    public string videoURL;             // Video clip for this listing

    public VideoPlayer player;  // Shared VideoPlayer in the scene

    // Method to play the video associated with this listing
    public void PlayVideo()
    {
        // Set the video clip to the VideoPlayer
        player.url = videoHost + videoURL;
        player.Prepare();

        // Optionally update the title, description, and thumbnail on the UI (if relevant)
        if (title != null)
        {
            Debug.Log("Playing: " + title.text);
        }

        if (description != null)
        {
            Debug.Log("Description: " + description.text);
        }

        if (thumbnail != null)
        {
            // You can do something with the thumbnail if needed
        }
        // Create a new material for the skybox
//        Material skyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));

        // Assign the VideoPlayer's texture to the skybox material
//        skyboxMaterial.SetTexture("_MainTex", player.texture);

        // Set the skybox for the current scene to the material
//        RenderSettings.skybox = skyboxMaterial;
        player.gameObject.SetActive(true);

        // Play the video
        player.Play();
    }

    public void StopVideo()
    {
        if (player != null && player.isPlaying)
        {
            player.Stop();
        }
    }
}
