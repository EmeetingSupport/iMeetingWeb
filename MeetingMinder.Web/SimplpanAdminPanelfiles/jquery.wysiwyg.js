!function (t) { function e(t, i) { return this instanceof e ? this.init(t, i) : new e(t, i) } t.fn.document = function () { var e = this[0]; return "iframe" == e.nodeName.toLowerCase() ? e.contentWindow.document : t(this) }, t.fn.documentSelection = function () { var t = this[0]; return t.contentWindow.document.selection ? t.contentWindow.document.selection.createRange().text : t.contentWindow.getSelection().toString() }, t.fn.wysiwyg = function (i) { if (arguments.length > 0 && arguments[0].constructor == String) { for (var s = arguments[0].toString(), n = [], o = 1; o < arguments.length; o++) n[o - 1] = arguments[o]; return s in e ? this.each(function () { t.data(this, "wysiwyg").designMode(), e[s].apply(this, n) }) : this } var r = {}; if (i && i.controls) { var r = i.controls; delete i.controls } var i = t.extend({ html: '<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"><html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en"><head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8">STYLE_SHEET</head><body style="color:#656769;font-family: Verdana, Arial, Helvetica, sans-serif !important; font-size: 13px; height: 100%; line-height: 1.5em !important;">INITIAL_CONTENT</body></html>', css: {}, debug: !1, autoSave: !0, rmUnwantedBr: !0, brIE: !0, controls: {}, messages: {} }, i); i.messages = t.extend(!0, i.messages, e.MSGS_EN), i.controls = t.extend(!0, i.controls, e.TOOLBAR); for (var a in r) a in i.controls ? t.extend(i.controls[a], r[a]) : i.controls[a] = r[a]; return this.each(function () { e(this, i) }) }, t.extend(e, { insertImage: function (i, s) { var n = t.data(this, "wysiwyg"); if (n.constructor == e && i && i.length > 0) if (s) { n.editorDoc.execCommand("insertImage", !1, "#jwysiwyg#"); var o = n.getElementByAttributeValue("img", "src", "#jwysiwyg#"); if (o) { o.src = i; for (var r in s) o.setAttribute(r, s[r]) } } else n.editorDoc.execCommand("insertImage", !1, i) }, createLink: function (i) { var s = t.data(this, "wysiwyg"); if (s.constructor == e && i && i.length > 0) { var n = t(s.editor).documentSelection(); n.length > 0 ? (s.editorDoc.execCommand("unlink", !1, []), s.editorDoc.execCommand("createLink", !1, i)) : s.options.messages.nonSelection && alert(s.options.messages.nonSelection) } }, setContent: function (e) { var i = t.data(this, "wysiwyg"); i.setContent(e), i.saveContent() }, clear: function () { var e = t.data(this, "wysiwyg"); e.setContent(""), e.saveContent() }, MSGS_EN: { nonSelection: "select the text you wish to link" }, TOOLBAR: { bold: { visible: !0, tags: ["b", "strong"], css: { fontWeight: "bold" } }, italic: { visible: !0, tags: ["i", "em"], css: { fontStyle: "italic" } }, strikeThrough: { visible: !1, tags: ["s", "strike"], css: { textDecoration: "line-through" } }, underline: { visible: !1, tags: ["u"], css: { textDecoration: "underline" } }, separator00: { visible: !1, separator: !0 }, justifyLeft: { visible: !1, css: { textAlign: "left" } }, justifyCenter: { visible: !1, tags: ["center"], css: { textAlign: "center" } }, justifyRight: { visible: !1, css: { textAlign: "right" } }, justifyFull: { visible: !1, css: { textAlign: "justify" } }, separator01: { visible: !1, separator: !0 }, indent: { visible: !1 }, outdent: { visible: !1 }, separator02: { visible: !1, separator: !0 }, subscript: { visible: !1, tags: ["sub"] }, superscript: { visible: !1, tags: ["sup"] }, separator03: { visible: !1, separator: !0 }, undo: { visible: !1 }, redo: { visible: !1 }, separator04: { visible: !1, separator: !0 }, insertOrderedList: { visible: !1, tags: ["ol"] }, insertUnorderedList: { visible: !1, tags: ["ul"] }, insertHorizontalRule: { visible: !1, tags: ["hr"] }, separator05: { separator: !0 }, createLink: { visible: !0, exec: function () { var e = t(this.editor).documentSelection(); if (e.length > 0) if (t.browser.msie) this.editorDoc.execCommand("createLink", !0, null); else { var i = prompt("URL", "http://"); i && i.length > 0 && (this.editorDoc.execCommand("unlink", !1, []), this.editorDoc.execCommand("createLink", !1, i)) } else this.options.messages.nonSelection && alert(this.options.messages.nonSelection) }, tags: ["a"] }, insertImage: { visible: !0, exec: function () { if (t.browser.msie) this.editorDoc.execCommand("insertImage", !0, null); else { var e = prompt("URL", "http://"); e && e.length > 0 && this.editorDoc.execCommand("insertImage", !1, e) } }, tags: ["img"] }, separator06: { separator: !0 }, h1mozilla: { visible: t.browser.mozilla, className: "h1", command: "heading", arguments: ["h1"], tags: ["h1"] }, h2mozilla: { visible: t.browser.mozilla, className: "h2", command: "heading", arguments: ["h2"], tags: ["h2"] }, h3mozilla: { visible: t.browser.mozilla, className: "h3", command: "heading", arguments: ["h3"], tags: ["h3"] }, h1: { visible: !t.browser.mozilla, className: "h1", command: "formatBlock", arguments: ["Heading 1"], tags: ["h1"] }, h2: { visible: !t.browser.mozilla, className: "h2", command: "formatBlock", arguments: ["Heading 2"], tags: ["h2"] }, h3: { visible: !t.browser.mozilla, className: "h3", command: "formatBlock", arguments: ["Heading 3"], tags: ["h3"] }, separator07: { visible: !1, separator: !0 }, cut: { visible: !1 }, copy: { visible: !1 }, paste: { visible: !1 }, separator08: { separator: !t.browser.msie }, increaseFontSize: { visible: !t.browser.msie, tags: ["big"] }, decreaseFontSize: { visible: !t.browser.msie, tags: ["small"] }, separator09: { separator: !0 }, html: { visible: !1, exec: function () { this.viewHTML ? (this.setContent(t(this.original).val()), t(this.original).hide()) : (this.saveContent(), t(this.original).show()), this.viewHTML = !this.viewHTML } }, removeFormat: { visible: !0, exec: function () { this.editorDoc.execCommand("removeFormat", !1, []), this.editorDoc.execCommand("unlink", !1, []) } } } }), t.extend(e.prototype, { original: null, options: {}, element: null, editor: null, init: function (e, i) { var s = this; this.editor = e, this.options = i || {}, t.data(e, "wysiwyg", this); var n = e.width || e.clientWidth, o = e.height || e.clientHeight; if ("textarea" == e.nodeName.toLowerCase()) { this.original = e, 0 == n && e.cols && (n = 8 * e.cols + 21), 0 == o && e.rows && (o = 16 * e.rows + 16); var r = this.editor = t("<iframe></iframe>").css({ minHeight: (o - 6).toString() + "px", width: (n - 8).toString() + "px" }).attr("id", t(e).attr("id") + "IFrame"); t.browser.msie && this.editor.css("height", o.toString() + "px") } var a = this.panel = t("<ul></ul>").addClass("panel"); this.appendControls(), this.element = t("<div></div>").css({ width: n > 0 ? n.toString() + "px" : "100%" }).addClass("wysiwyg").append(a).append(t("<div><!-- --></div>").css({ clear: "both" })).append(r), t(e).hide().before(this.element), this.viewHTML = !1, this.initialHeight = o - 8, this.initialContent = t(e).val(), this.initFrame(), 0 == this.initialContent.length && this.setContent(""), this.options.autoSave && t("form").submit(function () { s.saveContent() }), t("form").bind("reset", function () { s.setContent(s.initialContent), s.saveContent() }) }, initFrame: function () { var e = this, i = ""; this.options.css && this.options.css.constructor == String && (i = '<link rel="stylesheet" type="text/css" media="screen" href="' + this.options.css + '" />'), this.editorDoc = t(this.editor).document(), this.editorDoc_designMode = !1; try { this.editorDoc.designMode = "on", this.editorDoc_designMode = !0 } catch (s) { t(this.editorDoc).focus(function () { e.designMode() }) } this.editorDoc.open(), this.editorDoc.write(this.options.html.replace(/INITIAL_CONTENT/, this.initialContent).replace(/STYLE_SHEET/, i)), this.editorDoc.close(), this.editorDoc.contentEditable = "true", t.browser.msie && setTimeout(function () { t(e.editorDoc.body).css("border", "none") }, 0), t(this.editorDoc).click(function (t) { e.checkTargets(t.target ? t.target : t.srcElement) }), t(this.original).focus(function () { t(e.editorDoc.body).focus() }), this.options.autoSave && t(this.editorDoc).keydown(function () { e.saveContent() }).keyup(function () { e.saveContent() }).mousedown(function () { e.saveContent() }), this.options.css && setTimeout(function () { e.options.css.constructor == String || t(e.editorDoc).find("body").css(e.options.css) }, 0), t(this.editorDoc).keydown(function (i) { if (t.browser.msie && e.options.brIE && 13 == i.keyCode) { var s = e.getRange(); return s.pasteHTML("<br />"), s.collapse(!1), s.select(), !1 } }) }, designMode: function () { if (!this.editorDoc_designMode) try { this.editorDoc.designMode = "on", this.editorDoc_designMode = !0 } catch (t) { } }, getSelection: function () { return window.getSelection ? window.getSelection() : document.selection }, getRange: function () { var t = this.getSelection(); return t ? t.rangeCount > 0 ? t.getRangeAt(0) : t.createRange() : null }, getContent: function () { return t(t(this.editor).document()).find("body").html() }, setContent: function (e) { t(t(this.editor).document()).find("body").html(e) }, saveContent: function () { if (this.original) { var e = this.getContent(); this.options.rmUnwantedBr && (e = "<br>" == e.substr(-4) ? e.substr(0, e.length - 4) : e), t(this.original).val(e) } }, appendMenu: function (e, i, s, n) { var o = this, i = i || []; t("<li></li>").append(t("<a><!-- --></a>").addClass(s || e)).mousedown(function () { n ? n.apply(o) : o.editorDoc.execCommand(e, !1, i), o.options.autoSave && o.saveContent() }).appendTo(this.panel) }, appendMenuSeparator: function () { t('<li class="separator"></li>').appendTo(this.panel) }, appendControls: function () { for (var t in this.options.controls) { var e = this.options.controls[t]; e.separator ? e.visible !== !1 && this.appendMenuSeparator() : e.visible && this.appendMenu(e.command || t, e.arguments || [], e.className || e.command || t || "empty", e.exec) } }, checkTargets: function (e) { for (var i in this.options.controls) { var s = this.options.controls[i], n = s.className || s.command || i || "empty"; if (t("." + n, this.panel).removeClass("active"), s.tags) { var o = e; do { if (1 != o.nodeType) break; -1 != t.inArray(o.tagName.toLowerCase(), s.tags) && t("." + n, this.panel).addClass("active") } while (o = o.parentNode) } if (s.css) { var o = t(e); do { if (1 != o[0].nodeType) break; for (var r in s.css) o.css(r).toString().toLowerCase() == s.css[r] && t("." + n, this.panel).addClass("active") } while (o = o.parent()) } } }, getElementByAttributeValue: function (e, i, s) { for (var n = this.editorDoc.getElementsByTagName(e), o = 0; o < n.length; o++) { var r = n[o].getAttribute(i); if (t.browser.msie && (r = r.substr(r.length - s.length)), r == s) return n[o] } return !1 } }) }(jQuery);