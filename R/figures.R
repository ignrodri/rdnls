proc_t1val <- function(d, dataset, m, str, algo) {
	labelx <- sprintf("%s average (ms)", str)
	labely <- sprintf("%s difference %s - RDNLS (ms)", str, algo)
	test <- sprintf("test%d", m)
	f <- file.path(d, "transformedimages", dataset, sprintf("t1.%s.csv", test))
	dat <- read.csv(f)
	res <- data.frame()
	for (i in 1 : 100) {
		myrows <- sample(nrow(dat), 100)
		dat.sampled <- dat[myrows, ]
		res <- rbind(res, data.frame(map1 = mean(dat.sampled$map1), map2 = mean(dat.sampled$map2)))
	}
	myrows <- sample(nrow(dat), 1000)
	dat.sampled <- dat[myrows, ]
	g <- bland.altman.plot(res$map2, res$map1, mode = 2, graph.sys = "ggplot2", colour = "red")
	g <- blandr.draw(res$map1, res$map2, plotTitle = "", annotate = TRUE)
	g <- g + xlab(labelx) + ylab(labely) + theme(text = element_text(size = 20))
	print(g)
	ggsave(file.path(d, "figures", sprintf("trelvalues%d.tif", m)), device = 'tiff', plot = g)
	print(sprintf("test %d", m))
	print(mean(dat$map1))
	print(mean(dat$map2))
	ddif <- dat$map2 - dat$map1
	dm <- mean(ddif)
	dsd <- sd(ddif)
	print(sprintf("Bias %.2f Limits %.2f to %.2f", dm, dm - 1.96 * dsd, dm + 1.96 * dsd))
}

proc_ferror <- function(d, dataset, m) {
	f <- file.path(d, "transformedimages", dataset, sprintf("d.test%d.csv", m))
	dat <- read.csv(f)
	f <- file.path(d, "transformedimages", dataset, sprintf("d.test%d.txt", m))
	tt <- read.table(f, sep = "\t")
	mint <- min(tt[3,])
	maxt <- max(tt[3,])
	avet <- (mint + maxt) / 2
	dift <- maxt - mint
	mint <- avet - 0.7 * dift
	maxt <- avet + 0.7 * dift
	dat <- subset(dat, dat$x > mint)
	dat <- subset(dat, dat$x < maxt)
	g <- ggplot()
	g <- g + geom_point(data = dat, aes(x = dat$x, y = dat$y), colour = "red")
	g <- g + xlab("T1 (ms)") + ylab("Error (a.u.)") + theme(text = element_text(size = 20))
	print(g)
	ggsave(file.path(d, "figures", sprintf("ferror%d.tif", m)), device = 'tiff', plot = g)
	print(sprintf("test %d", m))
}

proc_t1 <- function(d) {
	library(ggplot2)
	library(BlandAltmanLeh)
	library(blandr)
	pdf(file.path(d, "trel.pdf"))
	proc_t1val(d, "dataset1", 1, 'T1', 'LM')
	proc_t1val(d, "dataset1", 2, 'T1', 'LM')
	proc_t1val(d, "dataset1", 3, 'T1', 'NM')
	proc_t1val(d, "dataset1", 4, 'T1', 'Split')
	proc_t1val(d, "dataset2", 5, 'T1', 'LM')
	proc_t1val(d, "dataset2", 6, 'T1', 'NM')
	proc_t1val(d, "dataset2", 7, 'T1', 'Split')
	proc_t1val(d, "dataset3", 8, 'T1', 'LM')
	proc_t1val(d, "dataset3", 9, 'T1', 'NM')
	proc_t1val(d, "dataset3", 10, 'T1', 'Split')
	proc_t1val(d, "dataset4", 11, 'T2', 'LM')
	proc_t1val(d, "dataset4", 12, 'T2', 'LM')
	proc_t1val(d, "dataset4", 13, 'T2', 'NM')
	proc_t1val(d, "dataset4", 14, 'T2', 'Split')
	proc_ferror(d, "dataset1", 1)
	proc_ferror(d, "dataset1", 2)
	proc_ferror(d, "dataset1", 3)
	proc_ferror(d, "dataset1", 4)
	proc_ferror(d, "dataset2", 5)
	proc_ferror(d, "dataset2", 6)
	proc_ferror(d, "dataset2", 7)
	proc_ferror(d, "dataset3", 8)
	proc_ferror(d, "dataset3", 9)
	proc_ferror(d, "dataset3", 10)
	dev.off()
}


