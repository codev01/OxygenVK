namespace OxygenVK.AppSource.LocaSettings.Attachments
{
	public class CompositeValueNames
	{
		public string TheNameOfTheCompoundUserDataAttachment { get; } = "userDataAttachmentsComposite";
		public string TheNameOfTheCompoundValueOfTheApplicationSettings { get; } = "applicationSettingsComposite";
		public UserDataAttachmentNames UserDataAttachmentNames { get; } = new UserDataAttachmentNames();
		public ApplicationSettingsAttachmentNames ApplicationSettingsAttachmentNames { get; } = new ApplicationSettingsAttachmentNames();
	}
}
