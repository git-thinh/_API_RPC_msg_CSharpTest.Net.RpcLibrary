<?xml version="1.0" encoding="utf-8"?>
<?mso-application progid="Excel.Sheet"?>
<xsl:stylesheet version="1.0" xmlns:html="http://www.w3.org/TR/REC-html40" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:x="urn:schemas-microsoft-com:office:excel"
    xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">

  <xsl:template match="root">
    <xsl:element name="sheetData">
      <xsl:apply-templates select="data"></xsl:apply-templates>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/">
    <Workbook>
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom" />
          <Borders />
          <Font />
          <Interior />
          <NumberFormat />
          <Protection />
        </Style>
        <Style ss:ID="s21">
          <Font ss:Size="22" ss:Bold="1" />
        </Style>
        <Style ss:ID="s22">
          <Font ss:Size="14" ss:Bold="1" />
        </Style>
        <Style ss:ID="s23">
          <Font ss:Size="12" ss:Bold="1" />
        </Style>
        <Style ss:ID="s24">
          <Font ss:Size="10" ss:Bold="1" />
        </Style>
      </Styles>

      <Worksheet ss:Name="Page1">
        <Table>
          <xsl:call-template name="XMLToXSL" />
        </Table>
      </Worksheet>
    </Workbook>

  </xsl:template>

  <xsl:template name="XMLToXSL">
    <Row>
      <Cell>
        <Data ss:Type="String">item</Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">tag</Data>
      </Cell>
    </Row>

    <xsl:for-each select="//data">
      <Row>
        <Cell>
          <Data ss:Type="String">
            <xsl:value-of select="item" />
          </Data>
        </Cell>
        <Cell>
          <Data ss:Type="String">
            <xsl:value-of select="tag" />
          </Data>
        </Cell>
      </Row>
    </xsl:for-each>
  </xsl:template>


  <xsl:template match="XMLToXSL">
    <xsl:variable name="rowID">
      <xsl:number value="position()" format="1"/>
    </xsl:variable>
    <xsl:element name="row">
      <xsl:attribute name="r">
        <xsl:value-of select="$rowID"/>
      </xsl:attribute>

      <xsl:for-each select="*">
        <xsl:element name="c">
          <xsl:variable name="colID">
            <xsl:number value="position()" format="A"/>
          </xsl:variable>
          <xsl:attribute name="r">
            <xsl:value-of  select="concat(string($colID),string($rowID))"/>
          </xsl:attribute>
          <xsl:attribute name="t">
            <xsl:text>inlineStr</xsl:text>
          </xsl:attribute>
          <xsl:element name="is">
            <xsl:element name="t">
              <xsl:value-of select="."/>
            </xsl:element>
          </xsl:element>
        </xsl:element>
      </xsl:for-each>

    </xsl:element>
  </xsl:template>



</xsl:stylesheet>