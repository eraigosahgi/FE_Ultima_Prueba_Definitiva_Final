﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace =
		"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2")]
	[System.Xml.Serialization.XmlRootAttribute("ApplicationResponse",
		Namespace = "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2", IsNullable = false)]
	public partial class ApplicationResponseType
	{

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "cac", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Cac { get; set; }

		[XmlAttribute(AttributeName = "cbc", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Cbc { get; set; }

		[XmlAttribute(AttributeName = "ccts", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ccts { get; set; }

		[XmlAttribute(AttributeName = "ds", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ds { get; set; }

		[XmlAttribute(AttributeName = "ext", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ext { get; set; }

		[XmlAttribute(AttributeName = "qdt", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Qdt { get; set; }

		[XmlAttribute(AttributeName = "udt", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Udt { get; set; }

		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }

		private UBLExtensionType[] uBLExtensionsField;

		private UBLVersionIDType uBLVersionIDField;

		private CustomizationIDType customizationIDField;

		private ProfileIDType profileIDField;

		private ProfileExecutionIDType profileExecutionIDField;

		private IDType idField;

		private UUIDType uUIDField;

		private IssueDateType issueDateField;

		private IssueTimeType issueTimeField;

		private ResponseDateType responseDateField;

		private ResponseTimeType responseTimeField;

		private NoteType[] noteField;

		private VersionIDType versionIDField;

		private SignatureType[] signatureField;

		private PartyType senderPartyField;

		private PartyType receiverPartyField;

		private DocumentResponseType[] documentResponseField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlArrayAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
		[System.Xml.Serialization.XmlArrayItemAttribute("UBLExtension", IsNullable = false)]
		public UBLExtensionType[] UBLExtensions
		{
			get { return this.uBLExtensionsField; }
			set { this.uBLExtensionsField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UBLVersionIDType UBLVersionID
		{
			get { return this.uBLVersionIDField; }
			set { this.uBLVersionIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CustomizationIDType CustomizationID
		{
			get { return this.customizationIDField; }
			set { this.customizationIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ProfileIDType ProfileID
		{
			get { return this.profileIDField; }
			set { this.profileIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ProfileExecutionIDType ProfileExecutionID
		{
			get { return this.profileExecutionIDField; }
			set { this.profileExecutionIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID
		{
			get { return this.idField; }
			set { this.idField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public UUIDType UUID
		{
			get { return this.uUIDField; }
			set { this.uUIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IssueDateType IssueDate
		{
			get { return this.issueDateField; }
			set { this.issueDateField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IssueTimeType IssueTime
		{
			get { return this.issueTimeField; }
			set { this.issueTimeField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ResponseDateType ResponseDate
		{
			get { return this.responseDateField; }
			set { this.responseDateField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ResponseTimeType ResponseTime
		{
			get { return this.responseTimeField; }
			set { this.responseTimeField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Note", Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NoteType[] Note
		{
			get { return this.noteField; }
			set { this.noteField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public VersionIDType VersionID
		{
			get { return this.versionIDField; }
			set { this.versionIDField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Signature", Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public SignatureType[] Signature
		{
			get { return this.signatureField; }
			set { this.signatureField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType SenderParty
		{
			get { return this.senderPartyField; }
			set { this.senderPartyField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public PartyType ReceiverParty
		{
			get { return this.receiverPartyField; }
			set { this.receiverPartyField = value; }
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DocumentResponse", Namespace =
			"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public DocumentResponseType[] DocumentResponse
		{
			get { return this.documentResponseField; }
			set { this.documentResponseField = value; }
		}
	}
}