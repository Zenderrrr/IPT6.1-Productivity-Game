<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { onMounted } from 'vue'
import { applyUIMode } from '@/utils/modeUI.ts'
import Footer from '@/components/layout/Footer.vue'

onMounted(async () => {
  applyUIMode()
})
</script>

<template>
  <div class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)] flex flex-col">
    <!-- Header -->
    <div class="w-full h-fit">
      <nav class="flex items-center justify-between max-w-[1400px] mx-auto w-full py-5">
        <Logo link="/" class="bg-transparent"></Logo>

        <RouterLink
          to="/"
          class="hover:text-[var(--primary-color)] hover:shadow-2xl hover:border-[var(--primary-color)] transition-all duration-100 text-sm px-4 py-2.5 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl text-[var(--text-color)] font-semibold flex items-center justify-center gap-2"
        >
          <div class="flex items-center justify-center">
            <i class="fa-regular fa-house"></i>
          </div>
          Zurück zur Startseite
        </RouterLink>
      </nav>
    </div>

    <!-- Main -->
    <main class="flex-1 max-w-[900px] mx-auto w-full px-5 py-16">
      <h1 class="text-[3.5rem] leading-tight font-extrabold text-[var(--text-color)] mb-10">
        Datenschutzerklärung
      </h1>

      <div class="privacy-content">
        <article>
          <p class="privacy-date">Stand: 1. September 2026</p><h2>Inhaltsübersicht</h2>
          <ul class="index"><li><a class="index-link" href="#m3">Verantwortlicher</a></li><li><a class="index-link" href="#mOverview">Übersicht der Verarbeitungen</a></li><li><a class="index-link" href="#m2427">Maßgebliche Rechtsgrundlagen</a></li><li><a class="index-link" href="#m27">Sicherheitsmaßnahmen</a></li><li><a class="index-link" href="#m25">Übermittlung von personenbezogenen Daten</a></li><li><a class="index-link" href="#m24">Internationale Datentransfers</a></li><li><a class="index-link" href="#m12">Allgemeine Informationen zur Datenspeicherung und Löschung</a></li><li><a class="index-link" href="#m10">Rechte der betroffenen Personen</a></li><li><a class="index-link" href="#m225">Bereitstellung des Onlineangebots und Webhosting</a></li><li><a class="index-link" href="#m134">Einsatz von Cookies</a></li><li><a class="index-link" href="#m328">Plug-ins und eingebettete Funktionen sowie Inhalte</a></li><li><a class="index-link" href="#m15">Änderung und Aktualisierung</a></li><li><a class="index-link" href="#m42">Begriffsdefinitionen</a></li></ul><h2 id="m3">Verantwortlicher</h2><p>Sanjivan Selvarasa<br>Oberhofstrasse 41<br>6020 Emmenbrücke, Schweiz<br><br>Egor Biriukov<br>Hirtenhofstrasse 16<br>6005 Luzern, Schweiz<br></p>
          <p>E-Mail-Adresse: <a href="mailto:kontakt@focusup.ch">kontakt@focusup.ch</a></p>
          <p>Impressum: <a href="https://www.focusup.ch/legal-notice" target="_blank">https://www.focusup.ch/legal-notice</a></p>

          <h2 id="mOverview">Übersicht der Verarbeitungen</h2><p>Die nachfolgende Übersicht fasst die Arten der verarbeiteten Daten und die Zwecke ihrer Verarbeitung zusammen und verweist auf die betroffenen Personen.</p><h3>Arten der verarbeiteten Daten</h3>
          <ul><li>Nutzungsdaten.</li><li>Meta-, Kommunikations- und Verfahrensdaten.</li><li>Protokolldaten.</li></ul><h3>Kategorien betroffener Personen</h3><ul><li>Nutzer.</li></ul><h3>Zwecke der Verarbeitung</h3><ul><li>Sicherheitsmaßnahmen.</li><li>Bereitstellung unseres Onlineangebotes und Nutzerfreundlichkeit.</li><li>Informationstechnische Infrastruktur.</li></ul><h2 id="m2427">Maßgebliche Rechtsgrundlagen</h2><p><strong>Maßgebliche Rechtsgrundlagen nach dem Schweizer Datenschutzgesetz: </strong>Wir bearbeiten Ihre Personendaten nach Maßgabe des Bundesgesetzes über den Datenschutz, kurz „Schweizer DSG“. Personendaten müssen rechtmäßig, nach Treu und Glauben und verhältnismäßig bearbeitet werden (Art. 6 Abs. 1 und 2 DSG). Sie dürfen nur zu einem bestimmten und für die betroffene Person erkennbaren Zweck beschafft und nur in einer mit diesem Zweck vereinbaren Weise bearbeitet werden (Art. 6 Abs. 3 DSG). Anders als die DSGVO knüpft das Schweizer DSG die Zulässigkeit der Bearbeitung durch private Verantwortliche grundsätzlich nicht an die Auswahl einer bestimmten Rechtsgrundlage aus einem gesetzlich festgelegten Katalog. Daher besteht grundsätzlich auch keine allgemeine Pflicht, für jede Bearbeitung eine solche Rechtsgrundlage in der Datenschutzerklärung anzugeben.</p>

          <h2 id="m27">Sicherheitsmaßnahmen</h2><p>Wir treffen nach Maßgabe der gesetzlichen Vorgaben unter Berücksichtigung des Stands der Technik, der Implementierungskosten und der Art, des Umfangs, der Umstände und der Zwecke der Verarbeitung sowie der unterschiedlichen Eintrittswahrscheinlichkeiten und des Ausmaßes der Bedrohung der Rechte und Freiheiten natürlicher Personen geeignete technische und organisatorische Maßnahmen, um ein dem Risiko angemessenes Schutzniveau zu gewährleisten.</p>
          <p>Zu den Maßnahmen gehören insbesondere die Sicherung der Vertraulichkeit, Integrität und Verfügbarkeit von Daten durch Kontrolle des physischen und elektronischen Zugangs zu den Daten als auch des sie betreffenden Zugriffs, der Eingabe, der Weitergabe, der Sicherung der Verfügbarkeit und ihrer Trennung. Des Weiteren haben wir Verfahren eingerichtet, die eine Wahrnehmung von Betroffenenrechten, die Löschung von Daten und Reaktionen auf die Gefährdung der Daten gewährleisten. Ferner berücksichtigen wir den Schutz personenbezogener Daten bereits bei der Entwicklung bzw. Auswahl von Hardware, Software sowie Verfahren entsprechend dem Prinzip des Datenschutzes, durch Technikgestaltung und durch datenschutzfreundliche Voreinstellungen.</p>
          <p>Sicherung von Online-Verbindungen durch TLS-/SSL-Verschlüsselungstechnologie (HTTPS): Um die Daten der Nutzer, die über unsere Online-Dienste übertragen werden, vor unerlaubten Zugriffen zu schützen, setzen wir auf die TLS-/SSL-Verschlüsselungstechnologie. Secure Sockets Layer (SSL) und Transport Layer Security (TLS) sind die Eckpfeiler der sicheren Datenübertragung im Internet. Diese Technologien verschlüsseln die Informationen, die zwischen der Website oder App und dem Browser des Nutzers (oder zwischen zwei Servern) übertragen werden, wodurch die Daten vor unbefugtem Zugriff geschützt sind. TLS, als die weiterentwickelte und sicherere Version von SSL, gewährleistet, dass alle Datenübertragungen den höchsten Sicherheitsstandards entsprechen. Wenn eine Website durch ein SSL-/TLS-Zertifikat gesichert ist, wird dies durch die Anzeige von HTTPS in der URL signalisiert. Dies dient als ein Indikator für die Nutzer, dass ihre Daten sicher und verschlüsselt übertragen werden.</p>

          <h2 id="m25">Übermittlung von personenbezogenen Daten</h2><p>Im Rahmen unserer Verarbeitung von personenbezogenen Daten kommt es vor, dass diese an andere Stellen, Unternehmen, rechtlich selbstständige Organisationseinheiten oder Personen übermittelt beziehungsweise ihnen gegenüber offengelegt werden. Zu den Empfängern dieser Daten können z. B. mit IT-Aufgaben beauftragte Dienstleister gehören oder Anbieter von Diensten und Inhalten, die in eine Website eingebunden sind. In solchen Fällen beachten wir die gesetzlichen Vorgaben und schließen insbesondere entsprechende Verträge bzw. Vereinbarungen, die dem Schutz Ihrer Daten dienen, mit den Empfängern Ihrer Daten ab.</p>

          <h2 id="m24">Internationale Datentransfers</h2><p>Bekanntgabe von Personendaten ins Ausland: Gemäß dem Schweizer DSG geben wir personenbezogene Daten nur dann ins Ausland bekannt, wenn ein angemessener Schutz der betroffenen Personen gewährleistet ist (Art. 16 Schweizer DSG). Sofern der Bundesrat keinen angemessenen Schutz festgestellt hat (Liste: <a href="https://www.bj.admin.ch/de/schweizerische-anerkennung-von-staaten-die-einen-angemessenen-datenschutz-gewaehrleisten" target="_blank">https://www.bj.admin.ch/de/schweizerische-anerkennung-von-staaten-die-einen-angemessenen-datenschutz-gewaehrleisten</a>), ergreifen wir alternative Sicherheitsmaßnahmen.</p>
          <p>Für Datenübermittlungen in die USA stützen wir uns vorrangig auf das Data Privacy Framework (DPF), welches durch einen Angemessenheitsbeschluss der Schweiz vom 15. September 2024 als sicherer Rechtsrahmen anerkannt wurde. Zusätzlich haben wir mit den jeweiligen Anbietern Standarddatenschutzklauseln abgeschlossen, die von der Eidgenössischen Datenschutz- und Öffentlichkeitsbeauftragten (EDÖB) genehmigt wurden und vertragliche Verpflichtungen zum Schutz Ihrer Daten festlegen.</p>
          <p>Diese zweifache Absicherung gewährleistet einen umfassenden Schutz Ihrer Daten: Das DPF bildet die primäre Schutzebene, während die Standarddatenschutzklauseln als zusätzliche Sicherheit dienen. Sollten sich Änderungen im Rahmen des DPF ergeben, greifen die Standarddatenschutzklauseln als zuverlässige Rückfalloption ein. So stellen wir sicher, dass Ihre Daten auch bei etwaigen politischen oder rechtlichen Veränderungen stets angemessen geschützt bleiben.</p>
          <p>Bei den einzelnen Diensteanbietern informieren wir Sie darüber, ob sie nach dem DPF zertifiziert sind und ob Standarddatenschutzklauseln vorliegen. Die Liste der zertifizierten Unternehmen sowie weitere Informationen zum DPF finden Sie auf der Website des US-Handelsministeriums unter <a href="https://www.dataprivacyframework.gov/" target="_blank">https://www.dataprivacyframework.gov/</a> (in englischer Sprache).</p>
          <p>Für Datenübermittlungen in andere Drittländer gelten entsprechende Sicherheitsmaßnahmen, einschließlich internationaler Verträge, spezifischer Garantien, von der EDÖB genehmigter Standarddatenschutzklauseln oder von der EDÖB oder einer zuständigen Datenschutzbehörde eines anderen Landes vorab anerkannter unternehmensinterner Datenschutzvorschriften.</p>

          <h2 id="m12">Allgemeine Informationen zur Datenspeicherung und Löschung</h2><p>Wir löschen personenbezogene Daten, die wir verarbeiten, gemäß den gesetzlichen Bestimmungen, sobald die zugrundeliegenden Einwilligungen widerrufen werden oder keine weiteren rechtlichen Grundlagen für die Verarbeitung bestehen. Dies betrifft Fälle, in denen der ursprüngliche Verarbeitungszweck entfällt oder die Daten nicht mehr benötigt werden. Ausnahmen von dieser Regelung bestehen, wenn gesetzliche Pflichten oder besondere Interessen eine längere Aufbewahrung oder Archivierung der Daten erfordern.</p>
          <p>Insbesondere müssen Daten, die aus handels- oder steuerrechtlichen Gründen aufbewahrt werden müssen oder deren Speicherung notwendig ist zur Rechtsverfolgung oder zum Schutz der Rechte anderer natürlicher oder juristischer Personen, entsprechend archiviert werden.</p>
          <p>Unsere Datenschutzhinweise enthalten zusätzliche Informationen zur Aufbewahrung und Löschung von Daten, die speziell für bestimmte Verarbeitungsprozesse gelten.</p>
          <p>Bei mehreren Angaben zur Aufbewahrungsdauer oder Löschungsfristen eines Datums, ist stets die längste Frist maßgeblich. Daten, die nicht mehr für den ursprünglich vorgesehenen Zweck, sondern aufgrund gesetzlicher Vorgaben oder anderer Gründe aufbewahrt werden, verarbeiten wir ausschließlich zu den Gründen, die ihre Aufbewahrung rechtfertigen.</p>
          <p>Aufbewahrung und Löschung von Daten: Die folgenden allgemeinen Fristen gelten für die Aufbewahrung und Archivierung nach dem Schweizer Recht:</p><ul> <li>10 Jahre - Aufbewahrungsfrist für Bücher und Aufzeichnungen, Jahresabschlüsse, Inventare, Lageberichte, Eröffnungsbilanzen, Buchungsbelege und Rechnungen sowie alle erforderlichen Arbeitsanweisungen und sonstigen Organisationsunterlagen (Art. 958f des Schweizerischen Obligationenrechts (OR)).</li><li>10 Jahre - Daten, die zur Berücksichtigung potenzieller Schadenersatzansprüche oder ähnlicher vertraglicher Ansprüche und Rechte notwendig sind, sowie für die Bearbeitung damit verbundener Anfragen, basierend auf früheren Geschäftserfahrungen und den üblichen Branchenpraktiken, werden für den Zeitraum der gesetzlichen Verjährungsfrist von zehn Jahren gespeichert, es sei denn, eine kürzere Frist von fünf Jahren ist maßgeblich, die in bestimmten Fällen einschlägig ist (Art. 127, 130 OR). Mit Ablauf von fünf Jahren verjähren die Forderungen für Miet-, Pacht- und Kapitalzinse sowie andere periodische Leistungen, aus Lieferung von Lebensmitteln, für Beköstigung und für Wirtsschulden, sowie aus Handwerksarbeit, Kleinverkauf von Waren, ärztlicher Besorgung, Berufsarbeiten von Anwälten, Rechtsagenten, Prokuratoren und Notaren und aus dem Arbeitsverhältnis von Arbeitnehmern (Art. 128 OR).</li> </ul>

          <h2 id="m10">Rechte der betroffenen Personen</h2><p>Rechte der betroffenen Personen nach dem Schweizer DSG: </p>
          <p>Ihnen stehen als betroffene Person nach Maßgabe der Vorgaben des Schweizer DSG die folgenden Rechte zu:</p><ul><li><strong>Recht auf Auskunft: </strong>Sie haben das Recht, eine Bestätigung darüber zu verlangen, ob Sie betreffende Personendaten verarbeitet werden, und auf Erhalt derjenigen Informationen, die erforderlich sind, damit Sie Ihre Rechte nach diesem Gesetz geltend machen können und eine transparente Datenbearbeitung gewährleistet ist.</li><li><strong>Recht auf Datenherausgabe oder -übertragung: </strong>Sie haben das Recht, die Herausgabe Ihrer Personendaten, die Sie uns bekanntgegeben haben, in einem gängigen elektronischen Format zu verlangen.</li><li><strong>Recht auf Berichtigung:</strong> Sie haben das Recht, die Berichtigung der Sie betreffenden unrichtigen Personendaten zu verlangen.</li><li><strong>Recht auf Widerspruch, Löschung und Vernichtung: </strong>Sie haben das Recht, der Verarbeitung Ihrer Daten zu widersprechen, sowie zu verlangen, dass die Sie betreffenden Personendaten gelöscht oder vernichtet werden.</li></ul>

          <h2 id="m225">Bereitstellung des Onlineangebots und Webhosting</h2><p>Wir verarbeiten die Daten der Nutzer, um ihnen unsere Online-Dienste zur Verfügung stellen zu können. Zu diesem Zweck verarbeiten wir die IP-Adresse des Nutzers, die notwendig ist, um die Inhalte und Funktionen unserer Online-Dienste an den Browser oder das Endgerät der Nutzer zu übermitteln.</p>
          <ul class="m-elements"><li><strong>Verarbeitete Datenarten:</strong> Nutzungsdaten (z. B. Seitenaufrufe und Verweildauer, Klickpfade, Nutzungsintensität und -frequenz, verwendete Gerätetypen und Betriebssysteme, Interaktionen mit Inhalten und Funktionen); Meta-, Kommunikations- und Verfahrensdaten (z. B. IP-Adressen, Zeitangaben, Identifikationsnummern, beteiligte Personen). Protokolldaten (z. B. Logfiles betreffend Logins oder den Abruf von Daten oder Zugriffszeiten.).</li><li><strong>Betroffene Personen:</strong> Nutzer (z. B. Webseitenbesucher, Nutzer von Onlinediensten).</li><li><strong>Zwecke der Verarbeitung und berechtigte Interessen:</strong> Bereitstellung unseres Onlineangebotes und Nutzerfreundlichkeit; Informationstechnische Infrastruktur (Betrieb und Bereitstellung von Informationssystemen und technischen Geräten (Computer, Server etc.)). Sicherheitsmaßnahmen.</li><li><strong>Aufbewahrung und Löschung:</strong> Löschung entsprechend Angaben im Abschnitt "Allgemeine Informationen zur Datenspeicherung und Löschung".</li></ul><p><strong>Weitere Hinweise zu Verarbeitungsprozessen, Verfahren und Diensten:</strong></p><ul class="m-elements"><li><strong>Bereitstellung Onlineangebot auf gemietetem Speicherplatz: </strong>Für die Bereitstellung unseres Onlineangebotes nutzen wir Speicherplatz, Rechenkapazität und Software, die wir von einem entsprechenden Serveranbieter (auch "Webhoster" genannt) mieten oder anderweitig beziehen; <span class=""><strong>Rechtsgrundlagen:</strong> Berechtigte Interessen (Art. 6 Abs. 1 S. 1 lit. f) DSGVO).</span></li><li><strong>Erhebung von Zugriffsdaten und Logfiles: </strong>Der Zugriff auf unser Onlineangebot wird in Form von sogenannten "Server-Logfiles" protokolliert. Zu den Serverlogfiles können die Adresse und der Name der abgerufenen Webseiten und Dateien, Datum und Uhrzeit des Abrufs, übertragene Datenmengen, Meldung über erfolgreichen Abruf, Browsertyp nebst Version, das Betriebssystem des Nutzers, Referrer URL (die zuvor besuchte Seite) und im Regelfall IP-Adressen und der anfragende Provider gehören. Die Serverlogfiles können zum einen zu Sicherheitszwecken eingesetzt werden, z. B. um eine Überlastung der Server zu vermeiden (insbesondere im Fall von missbräuchlichen Angriffen, sogenannten DDoS-Attacken), und zum anderen, um die Auslastung der Server und ihre Stabilität sicherzustellen; <span class=""><strong>Rechtsgrundlagen:</strong> Berechtigte Interessen (Art. 6 Abs. 1 S. 1 lit. f) DSGVO). </span><strong>Löschung von Daten:</strong> Logfile-Informationen werden für die Dauer von maximal 30 Tagen gespeichert und danach gelöscht oder anonymisiert. Daten, deren weitere Aufbewahrung zu Beweiszwecken erforderlich ist, sind bis zur endgültigen Klärung des jeweiligen Vorfalls von der Löschung ausgenommen.</li></ul>
          <h2 id="m134">Einsatz von Cookies</h2><p>Unter dem Begriff „Cookies" werden Funktionen, die Informationen auf Endgeräten der Nutzer speichern und aus ihnen auslesen, verstanden. Cookies können ferner in Bezug auf unterschiedliche Anliegen Einsatz finden, etwa zu Zwecken der Funktionsfähigkeit, der Sicherheit und des Komforts von Onlineangeboten sowie der Erstellung von Analysen der Besucherströme. Wir verwenden Cookies gemäß den gesetzlichen Vorschriften. Dazu holen wir, wenn erforderlich, vorab die Zustimmung der Nutzer ein. Ist eine Zustimmung nicht notwendig, setzen wir auf unsere berechtigten Interessen. Dies gilt, wenn das Speichern und Auslesen von Informationen unerlässlich ist, um ausdrücklich angeforderte Inhalte und Funktionen bereitstellen zu können. Dazu zählen etwa die Speicherung von Einstellungen sowie die Sicherstellung der Funktionalität und Sicherheit unseres Onlineangebots. Die Einwilligung kann jederzeit widerrufen werden. Wir informieren klar über deren Umfang und welche Cookies genutzt werden.</p>
          <p><strong>Hinweise zu datenschutzrechtlichen Rechtsgrundlagen: </strong>Ob wir personenbezogene Daten mithilfe von Cookies verarbeiten, hängt von einer Einwilligung ab. Liegt eine Einwilligung vor, dient sie als Rechtsgrundlage. Ohne Einwilligung stützen wir uns auf unsere berechtigten Interessen, die vorstehend in diesem Abschnitt und im Kontext der jeweiligen Dienste und Verfahren erläutert sind.</p>
          <p><strong>Speicherdauer: </strong>Im Hinblick auf die Speicherdauer werden die folgenden Arten von Cookies unterschieden:</p><ul><li><strong>Temporäre Cookies (auch: Session- oder Sitzungscookies):</strong> Temporäre Cookies werden spätestens gelöscht, nachdem ein Nutzer ein Onlineangebot verlassen und sein Endgerät (z. B. Browser oder mobile Applikation) geschlossen hat.</li><li><strong>Permanente Cookies:</strong> Permanente Cookies bleiben auch nach dem Schließen des Endgeräts gespeichert. So können beispielsweise der Log-in-Status gespeichert und bevorzugte Inhalte direkt angezeigt werden, wenn der Nutzer eine Website erneut besucht. Ebenso können die mithilfe von Cookies erhobenen Nutzerdaten zur Reichweitenmessung Verwendung finden. Sofern wir Nutzern keine expliziten Angaben zur Art und Speicherdauer von Cookies mitteilen (z. B. im Rahmen der Einholung der Einwilligung), sollten sie davon ausgehen, dass diese permanent sind und die Speicherdauer bis zu zwei Jahre betragen kann.</li></ul><p><strong>Allgemeine Hinweise zum Widerruf und Widerspruch (Opt-out): </strong>Nutzer können die von ihnen abgegebenen Einwilligungen jederzeit widerrufen und zudem einen Widerspruch gegen die Verarbeitung entsprechend den gesetzlichen Vorgaben, auch mittels der Privatsphäre-Einstellungen ihres Browsers, erklären.</p>
          <ul class="m-elements"><li><strong>Verarbeitete Datenarten:</strong> Meta-, Kommunikations- und Verfahrensdaten (z. B. IP-Adressen, Zeitangaben, Identifikationsnummern, beteiligte Personen).</li><li><strong>Betroffene Personen:</strong> Nutzer (z. B. Webseitenbesucher, Nutzer von Onlinediensten).</li></ul>
          <h2 id="m328">Plug-ins und eingebettete Funktionen sowie Inhalte</h2><p>Wir binden Funktions- und Inhaltselemente in unser Onlineangebot ein, die von den Servern ihrer jeweiligen Anbieter (nachfolgend als „Drittanbieter" bezeichnet) bezogen werden. Dabei kann es sich zum Beispiel um Grafiken, Videos oder Stadtpläne handeln (nachfolgend einheitlich als „Inhalte" bezeichnet).</p>
          <p>Die Einbindung setzt immer voraus, dass die Drittanbieter dieser Inhalte die IP-Adresse der Nutzer verarbeiten, da sie ohne IP-Adresse die Inhalte nicht an deren Browser senden könnten. Die IP-Adresse ist damit für die Darstellung dieser Inhalte oder Funktionen erforderlich. Wir bemühen uns, nur solche Inhalte zu verwenden, deren jeweilige Anbieter die IP-Adresse lediglich zur Auslieferung der Inhalte anzuwenden. Drittanbieter können ferner sogenannte Pixel-Tags (unsichtbare Grafiken, auch als „Web Beacons" bezeichnet) für statistische oder Marketingzwecke einsetzen. Durch die „Pixel-Tags" können Informationen, wie etwa der Besucherverkehr auf den Seiten dieser Website, ausgewertet werden. Die pseudonymen Informationen können darüber hinaus in Cookies auf dem Gerät der Nutzer gespeichert werden und unter anderem technische Auskünfte zum Browser und zum Betriebssystem, zu verweisenden Websites, zur Besuchszeit sowie weitere Angaben zur Nutzung unseres Onlineangebots enthalten, aber auch mit solchen Informationen aus anderen Quellen verbunden werden.</p>
          <p><strong>Hinweise zu Rechtsgrundlagen:</strong> Sofern wir die Nutzer um ihre Einwilligung in den Einsatz der Drittanbieter bitten, stellt die Rechtsgrundlage der Datenverarbeitung die Erlaubnis dar. Ansonsten werden die Nutzerdaten auf Grundlage unserer berechtigten Interessen (d. h. Interesse an effizienten, wirtschaftlichen und empfängerfreundlichen Leistungen) verarbeitet. In diesem Zusammenhang möchten wir Sie auch auf die Informationen zur Verwendung von Cookies in dieser Datenschutzerklärung hinweisen.</p>
          <ul class="m-elements"><li><strong>Verarbeitete Datenarten:</strong> Nutzungsdaten (z. B. Seitenaufrufe und Verweildauer, Klickpfade, Nutzungsintensität und -frequenz, verwendete Gerätetypen und Betriebssysteme, Interaktionen mit Inhalten und Funktionen). Meta-, Kommunikations- und Verfahrensdaten (z. B. IP-Adressen, Zeitangaben, Identifikationsnummern, beteiligte Personen).</li><li><strong>Betroffene Personen:</strong> Nutzer (z. B. Webseitenbesucher, Nutzer von Onlinediensten).</li><li><strong>Zwecke der Verarbeitung und berechtigte Interessen:</strong> Bereitstellung unseres Onlineangebotes und Nutzerfreundlichkeit.</li><li><strong>Aufbewahrung und Löschung:</strong> Löschung entsprechend Angaben im Abschnitt "Allgemeine Informationen zur Datenspeicherung und Löschung". Speicherung von Cookies von bis zu 2 Jahren (Sofern nicht anders angegeben, können Cookies und ähnliche Speichermethoden für einen Zeitraum von zwei Jahren auf den Geräten der Nutzer gespeichert werden.).</li></ul><p><strong>Weitere Hinweise zu Verarbeitungsprozessen, Verfahren und Diensten:</strong></p><ul class="m-elements"><li><strong>Google Fonts (Bezug vom Google Server): </strong>Bezug von Schriften (und Symbolen) zum Zwecke einer technisch sicheren, wartungsfreien und effizienten Nutzung von Schriften und Symbolen im Hinblick auf Aktualität und Ladezeiten, deren einheitliche Darstellung und Berücksichtigung möglicher lizenzrechtlicher Beschränkungen. Dem Anbieter der Schriftarten wird die IP-Adresse des Nutzers mitgeteilt, damit die Schriftarten im Browser des Nutzers zur Verfügung gestellt werden können. Darüber hinaus werden technische Daten (Spracheinstellungen, Bildschirmauflösung, Betriebssystem, verwendete Hardware) übermittelt, die für die Bereitstellung der Schriften in Abhängigkeit von den verwendeten Geräten und der technischen Umgebung notwendig sind. Diese Daten können auf einem Server des Anbieters der Schriftarten in den USA verarbeitet werden - Beim Besuch unseres Onlineangebotes senden die Browser der Nutzer ihre Browser HTTP-Anfragen an die Google Fonts Web API (d. h. eine Softwareschnittstelle für den Abruf der Schriftarten). Die Google Fonts Web API stellt den Nutzern die Cascading Style Sheets (CSS) von Google Fonts und danach die in der CCS angegebenen Schriftarten zur Verfügung. Zu diesen HTTP-Anfragen gehören (1) die vom jeweiligen Nutzer für den Zugriff auf das Internet verwendete IP-Adresse, (2) die angeforderte URL auf dem Google-Server und (3) die HTTP-Header, einschließlich des User-Agents, der die Browser- und Betriebssystemversionen der Websitebesucher beschreibt, sowie die Verweis-URL (d. h. die Webseite, auf der die Google-Schriftart angezeigt werden soll). IP-Adressen werden weder auf Google-Servern protokolliert noch gespeichert und sie werden nicht analysiert. Die Google Fonts Web API protokolliert Details der HTTP-Anfragen (angeforderte URL, User-Agent und Verweis-URL). Der Zugriff auf diese Daten ist eingeschränkt und streng kontrolliert. Die angeforderte URL identifiziert die Schriftfamilien, für die der Nutzer Schriftarten laden möchte. Diese Daten werden protokolliert, damit Google bestimmen kann, wie oft eine bestimmte Schriftfamilie angefordert wird. Bei der Google Fonts Web API muss der User-Agent die Schriftart anpassen, die für den jeweiligen Browsertyp generiert wird. Der User-Agent wird in erster Linie zum Debugging protokolliert und verwendet, um aggregierte Nutzungsstatistiken zu generieren, mit denen die Beliebtheit von Schriftfamilien gemessen wird. Diese zusammengefassten Nutzungsstatistiken werden auf der Seite „Analysen" von Google Fonts veröffentlicht. Schließlich wird die Verweis-URL protokolliert, sodass die Daten für die Wartung der Produktion verwendet und ein aggregierter Bericht zu den Top-Integrationen basierend auf der Anzahl der Schriftartenanfragen generiert werden kann. Google verwendet laut eigener Auskunft keine der von Google Fonts erfassten Informationen, um Profile von Endnutzern zu erstellen oder zielgerichtete Anzeigen zu schalten; <strong>Dienstanbieter:</strong> Google Ireland Limited, Gordon House, Barrow Street, Dublin 4, Irland; <span class=""><strong>Rechtsgrundlagen:</strong> Berechtigte Interessen (Art. 6 Abs. 1 S. 1 lit. f) DSGVO); </span><strong>Website:</strong> <a href="https://fonts.google.com/" target="_blank">https://fonts.google.com/</a>; <strong>Datenschutzerklärung:</strong> <a href="https://business.safety.google/privacy/" target="_blank">https://business.safety.google/privacy/</a>; <strong>Grundlage Drittlandtransfers:</strong> Data Privacy Framework (DPF), Data Privacy Framework (DPF). <strong>Weitere Informationen:</strong> <a href="https://fonts.google.com/faq?hl=de#privacy" target="_blank">https://fonts.google.com/faq?hl=de#privacy</a>.</li><li><strong>Font Awesome (Bereitstellung auf eigenem Server): </strong>Darstellung von Schriftarten und Symbolen; <strong>Dienstanbieter:</strong> Die Font Awesome Icons werden auf unserem Server gehostet, es werden keine Daten an den Anbieter von Font Awesome übermittelt; <span class=""><strong>Rechtsgrundlagen:</strong> Berechtigte Interessen (Art. 6 Abs. 1 S. 1 lit. f) DSGVO).</span></li></ul>
          <h2 id="m15">Änderung und Aktualisierung</h2><p>Wir bitten Sie, sich regelmäßig über den Inhalt unserer Datenschutzerklärung zu informieren. Wir passen die Datenschutzerklärung an, sobald die Änderungen der von uns durchgeführten Datenverarbeitungen dies erforderlich machen. Wir informieren Sie, sobald durch die Änderungen eine Mitwirkungshandlung Ihrerseits (z. B. Einwilligung) oder eine sonstige individuelle Benachrichtigung erforderlich wird.</p>
          <p>Sofern wir in dieser Datenschutzerklärung Adressen und Kontaktinformationen von Unternehmen und Organisationen angeben, bitten wir zu beachten, dass die Adressen sich über die Zeit ändern können und bitten die Angaben vor Kontaktaufnahme zu prüfen.</p>

          <h2 id="m42">Begriffsdefinitionen</h2><p>In diesem Abschnitt erhalten Sie eine Übersicht über die in dieser Datenschutzerklärung verwendeten Begrifflichkeiten. Soweit die Begrifflichkeiten gesetzlich definiert sind, gelten deren gesetzliche Definitionen. Die nachfolgenden Erläuterungen sollen dagegen vor allem dem Verständnis dienen.</p>
          <ul class="glossary"><li><strong>Meta-, Kommunikations- und Verfahrensdaten:</strong> Meta-, Kommunikations- und Verfahrensdaten sind Kategorien, die Informationen über die Art und Weise enthalten, wie Daten verarbeitet, übermittelt und verwaltet werden. Meta-Daten, auch bekannt als Daten über Daten, umfassen Informationen, die den Kontext, die Herkunft und die Struktur anderer Daten beschreiben. Sie können Angaben zur Dateigröße, dem Erstellungsdatum, dem Autor eines Dokuments und den Änderungshistorien beinhalten. Kommunikationsdaten erfassen den Austausch von Informationen zwischen Nutzern über verschiedene Kanäle, wie E-Mail-Verkehr, Anrufprotokolle, Nachrichten in sozialen Netzwerken und Chat-Verläufe, inklusive der beteiligten Personen, Zeitstempel und Übertragungswege. Verfahrensdaten beschreiben die Prozesse und Abläufe innerhalb von Systemen oder Organisationen, einschließlich Workflow-Dokumentationen, Protokolle von Transaktionen und Aktivitäten, sowie Audit-Logs, die zur Nachverfolgung und Überprüfung von Vorgängen verwendet werden. </li><li><strong>Nutzungsdaten:</strong> Nutzungsdaten beziehen sich auf Informationen, die erfassen, wie Nutzer mit digitalen Produkten, Dienstleistungen oder Plattformen interagieren. Diese Daten umfassen eine breite Palette von Informationen, die aufzeigen, wie Nutzer Anwendungen nutzen, welche Funktionen sie bevorzugen, wie lange sie auf bestimmten Seiten verweilen und über welche Pfade sie durch eine Anwendung navigieren. Nutzungsdaten können auch die Häufigkeit der Nutzung, Zeitstempel von Aktivitäten, IP-Adressen, Geräteinformationen und Standortdaten einschließen. Sie sind besonders wertvoll für die Analyse des Nutzerverhaltens, die Optimierung von Benutzererfahrungen, das Personalisieren von Inhalten und das Verbessern von Produkten oder Dienstleistungen. Darüber hinaus spielen Nutzungsdaten eine entscheidende Rolle beim Erkennen von Trends, Vorlieben und möglichen Problembereichen innerhalb digitaler Angebote </li><li><strong>Personenbezogene Daten:</strong> "Personenbezogene Daten" sind alle Informationen, die sich auf eine identifizierte oder identifizierbare natürliche Person (im Folgenden "betroffene Person") beziehen; als identifizierbar wird eine natürliche Person angesehen, die direkt oder indirekt, insbesondere mittels Zuordnung zu einer Kennung wie einem Namen, zu einer Kennnummer, zu Standortdaten, zu einer Online-Kennung (z. B. Cookie) oder zu einem oder mehreren besonderen Merkmalen identifiziert werden kann, die Ausdruck der physischen, physiologischen, genetischen, psychischen, wirtschaftlichen, kulturellen oder sozialen Identität dieser natürlichen Person sind. </li><li><strong>Protokolldaten:</strong> Protokolldaten sind Informationen über Ereignisse oder Aktivitäten, die in einem System oder Netzwerk protokolliert wurden. Diese Daten enthalten typischerweise Informationen wie Zeitstempel, IP-Adressen, Benutzeraktionen, Fehlermeldungen und andere Details über die Nutzung oder den Betrieb eines Systems. Protokolldaten werden oft zur Analyse von Systemproblemen, zur Sicherheitsüberwachung oder zur Erstellung von Leistungsberichten verwendet. </li><li><strong>Verantwortlicher:</strong> Als "Verantwortlicher" wird die natürliche oder juristische Person, Behörde, Einrichtung oder andere Stelle, die allein oder gemeinsam mit anderen über die Zwecke und Mittel der Verarbeitung von personenbezogenen Daten entscheidet, bezeichnet. </li><li><strong>Verarbeitung:</strong> "Verarbeitung" ist jeder mit oder ohne Hilfe automatisierter Verfahren ausgeführte Vorgang oder jede solche Vorgangsreihe im Zusammenhang mit personenbezogenen Daten. Der Begriff reicht weit und umfasst praktisch jeden Umgang mit Daten, sei es das Erheben, das Auswerten, das Speichern, das Übermitteln oder das Löschen. </li></ul><p class="seal"><a href="https://datenschutz-generator.de/" title="Rechtstext von Dr. Schwenke - für weitere Informationen bitte anklicken." target="_blank" rel="noopener noreferrer nofollow">Erstellt mit kostenlosem Datenschutz-Generator.de von Dr. Thomas Schwenke</a></p>
        </article>
      </div>
    </main>

    <!-- Footer -->
    <Footer></Footer>
  </div>
</template>

<style scoped>
.privacy-content {
  color: var(--text-color-light);
  font-size: 1.05rem;
  line-height: 1.8;
  overflow-wrap: anywhere;
}

.privacy-content h2 {
  margin: 3.5rem 0 1.25rem;
  padding-bottom: 0.75rem;
  border-bottom: 1px solid var(--border-color);
  color: var(--text-color);
  font-size: clamp(1.5rem, 3vw, 2rem);
  font-weight: 800;
  line-height: 1.3;
  scroll-margin-top: 6rem;
}

.privacy-content h3 {
  margin: 2rem 0 0.75rem;
  color: var(--text-color);
  font-size: 1.25rem;
  font-weight: 700;
  line-height: 1.4;
}

.privacy-content p {
  margin: 0 0 1.25rem;
}

.privacy-content strong {
  color: var(--text-color);
  font-weight: 700;
}

.privacy-content ul {
  margin: 0 0 1.5rem;
  padding-left: 1.5rem;
  list-style: disc;
}

.privacy-content li + li {
  margin-top: 0.5rem;
}

.privacy-content a {
  color: var(--primary-color);
  font-weight: 500;
  text-decoration: underline;
  text-underline-offset: 3px;
}

.privacy-content a:hover {
  color: var(--secondary-color);
}

.privacy-date {
  margin-bottom: 2.5rem !important;
  color: var(--text-color-light);
  font-size: 0.95rem;
}

/* Inhaltsübersicht */
.privacy-content .index {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 0.5rem 1.5rem;
  margin: 1rem 0 3rem;
  padding: 1.5rem;
  list-style: none;
  background: var(--surface-color);
  border: 1px solid var(--border-color);
  border-radius: 1rem;
}

.privacy-content .index li {
  margin: 0;
}

.privacy-content .index-link {
  display: block;
  padding: 0.25rem 0;
  text-decoration: none;
}

/* Detailinformationen als Karten */
.privacy-content .m-elements {
  display: grid;
  gap: 0.75rem;
  padding: 0;
  list-style: none;
}

.privacy-content .m-elements > li {
  margin: 0;
  padding: 1rem 1.25rem;
  background: var(--surface-color);
  border: 1px solid var(--border-color);
  border-radius: 0.75rem;
}

/* Generator-Hinweis */
.privacy-content .seal {
  margin-top: 4rem;
  padding-top: 1.5rem;
  border-top: 1px solid var(--border-color);
  font-size: 0.875rem;
}

@media (max-width: 640px) {
  .privacy-content {
    font-size: 1rem;
    line-height: 1.7;
  }

  .privacy-content .index {
    grid-template-columns: 1fr;
  }
}
</style>
