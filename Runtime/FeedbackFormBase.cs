using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SBG.FeedbackForm
{
	public abstract class FeedbackFormBase : MonoBehaviour
	{
        protected struct FormEntry
        {
            public string FormEntryID;
            public string Data;

            public FormEntry(string id, string data)
            {
                FormEntryID = id;
                Data = data;
            }
        }

        [SerializeField] protected string formURL;
		
		protected abstract List<FormEntry> GetFormEntries();

		public void SendForm()
		{
			var entries = GetFormEntries();

			if (entries == null || entries.Count == 0)
			{
				Debug.Log("No form entries to send!");
				return;
			}

			StartCoroutine(CO_Post(entries));
		}

		private IEnumerator CO_Post(List<FormEntry> entries)
		{
			WWWForm form = new WWWForm();

			foreach (var entry in entries)
			{
				if (string.IsNullOrEmpty(entry.Data)) continue;
                form.AddField($"entry.{entry.FormEntryID}", entry.Data);
            }

			using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
			{
				yield return www.SendWebRequest();

				if (www.result == UnityWebRequest.Result.Success)
				{
					Debug.Log("Feedback Form submitted!");
				}
				else
				{
                    Debug.LogError($"Feedback Form Error: {www.error}");
                }
            }
		}
	}
}