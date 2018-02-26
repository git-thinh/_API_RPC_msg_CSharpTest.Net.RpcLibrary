<?xml version="1.0" encoding="ISO-8859-1"?>

<?mso-application progid="Excel.Sheet"?>
<xsl:stylesheet version="1.0"
    xmlns:html="http://www.w3.org/TR/REC-html40"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:x="urn:schemas-microsoft-com:office:excel"
    xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">


  <xsl:param name="header1">Header1</xsl:param>
  <xsl:param name="header2">Header2</xsl:param>
  <xsl:param name="header3">Header3</xsl:param>
  <xsl:param name="header4">Header4</xsl:param>
  <xsl:param name="header5">Header5</xsl:param>
  <xsl:param name="header6">Header6</xsl:param>
  <xsl:param name="header7">Header7</xsl:param>
  <xsl:param name="header8">Header8</xsl:param>

  <xsl:template match="rows">

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
        <Style ss:ID="columnheaders">
          <Font ss:Size="12" ss:Bold="1" />
        </Style>
        <Style ss:ID="s22">
          <Font ss:Size="14" ss:Bold="1" />
        </Style>
        <Style ss:ID="s23">
          <Font ss:Size="10"  />
        </Style>
        <Style ss:ID="s24">
          <Font ss:Size="10" ss:Bold="1" />
        </Style>
      </Styles>

      <Worksheet ss:Name="data">
        <Table>
          <Column ss:AutoFitWidth="0" ss:Width="300" />
          <Column ss:AutoFitWidth="0" ss:Width="95" />
          <Column ss:AutoFitWidth="0" ss:Width="95" />
          <Column ss:AutoFitWidth="0" ss:Width="175" />
          <Column ss:AutoFitWidth="0" ss:Width="186" />
          <Column ss:AutoFitWidth="0" ss:Width="185" />
          <Column ss:AutoFitWidth="0" ss:Width="113" />
          <Column ss:AutoFitWidth="0" ss:Width="133" />


          <Row ss:AutoFitHeight="0" ss:Height="18">
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header1"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header2"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header3"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header4"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header5"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header6"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header7"/>
              </Data>
            </Cell>
            <Cell ss:StyleID="columnheaders">
              <Data ss:Type="String">
                <xsl:value-of select="$header8"/>
              </Data>
            </Cell>
          </Row>

          <xsl:apply-templates select="row"/>

        </Table>
      </Worksheet>

    </Workbook>

  </xsl:template>


  <xsl:template match="row">
    <xsl:variable name="rowID">
      <xsl:number level="any" format="1"/>
    </xsl:variable>
    <Row ss:AutoFitHeight="0" ss:Height="18">
      <xsl:for-each select="cell">
        <xsl:variable name="colID">
          <xsl:number value="position()" format="A"/>
        </xsl:variable>
        <Cell ss:StyleID="s23">
          <Data ss:Type="String">
            <xsl:value-of select="translate(.,'?','&#8364;')"/>
          </Data>
        </Cell>
      </xsl:for-each>
    </Row>

    <xsl:apply-templates select="row"/>
  </xsl:template>


</xsl:stylesheet>