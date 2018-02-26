<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:p="urn:somePhoneBook" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">

  <xsl:template match="/">
    <ss:Workbook>
      <ss:Styles>
        <ss:Style ss:ID="1">"
          <ss:Font ss:Bold="1"/>"
        </ss:Style>
      </ss:Styles>
      <ss:Worksheet ss:Name="Sheet1">"
        <xsl:apply-templates />
      </ss:Worksheet>
    </ss:Workbook>
  </xsl:template>
  
  <xsl:template match="p:sheetData">
    <ss:Table>
      <ss:Row ss:StyleID="1">"
        <ss:Cell>
          <ss:Data ss:Type="String">Item</ss:Data>
        </ss:Cell>
        <ss:Cell>
          <ss:Data ss:Type="String">Tag</ss:Data>
        </ss:Cell>
        <ss:Cell>
          <ss:Data ss:Type="String">Type</ss:Data>
        </ss:Cell>
      </ss:Row>
      <xsl:apply-templates />
    </ss:Table>
  </xsl:template>

  <xsl:template match="p:row">
    <ss:Row>
      <ss:Cell>
        <ss:Data ss:Type="String">
          <xsl:value-of select="p:item" />
        </ss:Data>
      </ss:Cell>
      <ss:Cell>
        <ss:Data ss:Type="String">
          <xsl:value-of select="p:tag" />
        </ss:Data>
      </ss:Cell>
      <ss:Cell>
        <ss:Data ss:Type="String">
          <xsl:value-of select="p:type" />
        </ss:Data>
      </ss:Cell>
    </ss:Row>
  </xsl:template>

</xsl:stylesheet>