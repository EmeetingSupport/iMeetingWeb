jQuery && function (e) { e.extend(e.fn, { fileTree: function (a, i) { if (!a) var a = {}; void 0 == a.root && (a.root = "/"), void 0 == a.script && (a.script = "jqueryFileTree.php"), void 0 == a.folderEvent && (a.folderEvent = "click"), void 0 == a.expandSpeed && (a.expandSpeed = 500), void 0 == a.collapseSpeed && (a.collapseSpeed = 500), void 0 == a.expandEasing && (a.expandEasing = null), void 0 == a.collapseEasing && (a.collapseEasing = null), void 0 == a.multiFolder && (a.multiFolder = !0), void 0 == a.loadMessage && (a.loadMessage = "Loading..."), e(this).each(function () { function s(i, s) { e(i).addClass("wait"), e(".jqueryFileTree.start").remove(), e.post(a.script, { dir: s }, function (n) { e(i).find(".start").html(""), e(i).removeClass("wait").append(n), a.root == s ? e(i).find("UL:hidden").show() : e(i).find("UL:hidden").slideDown({ duration: a.expandSpeed, easing: a.expandEasing }), d(i) }) } function d(d) { e(d).find("LI A").bind(a.folderEvent, function () { return e(this).parent().hasClass("directory") ? e(this).parent().hasClass("collapsed") ? (a.multiFolder || (e(this).parent().parent().find("UL").slideUp({ duration: a.collapseSpeed, easing: a.collapseEasing }), e(this).parent().parent().find("LI.directory").removeClass("expanded").addClass("collapsed")), e(this).parent().find("UL").remove(), s(e(this).parent(), escape(e(this).attr("rel").match(/.*\//))), e(this).parent().removeClass("collapsed").addClass("expanded")) : (e(this).parent().find("UL").slideUp({ duration: a.collapseSpeed, easing: a.collapseEasing }), e(this).parent().removeClass("expanded").addClass("collapsed")) : i(e(this).attr("rel")), !1 }), "click" != a.folderEvent.toLowerCase && e(d).find("LI A").bind("click", function () { return !1 }) } e(this).html('<ul class="jqueryFileTree start"><li class="wait">' + a.loadMessage + "<li></ul>"), s(e(this), escape(a.root)) }) } }) }(jQuery);